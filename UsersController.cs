using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(DAL.UserInfo.Instance.GetCount());
        }
        // PUT api/<UsersController>/5

        [HttpPut("{username}")]
        public ActionResult getUser(string username)
        {
            var result = DAL.UserInfo.Instance.GetModel(username);
            if (result != null)
                return Ok(Result.Ok(result));
            else
                return Ok(Result.Err("用户名不存在"));
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult Post([FromBody]Model.UserInfo users)
        {
            try
            {
                int n = DAL.UserInfo.Instance.Add(users);
                return Ok(Result.Ok("添加成功"));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("primary"))
                    return Ok(Result.Err("用户名已存在"));
                else if (ex.Message.ToLower().Contains("null"))
                    return Ok(Result.Err("用户名、密码、身份证不能为空"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPut]
        public ActionResult Put([FromBody]Model.UserInfo users)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Update(users);
                if (n != 0)
                    return Ok(Result.Ok("修改成功"));
                else
                    return Ok(Result.Err("用户名不存在"));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("null"))
                    return Ok(Result.Err("密码、身份不能为空"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }



        // DELETE api/<UsersController>/5
        [HttpDelete("{username}")]
        public ActionResult Delete(string username)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Delete(username);
                if (n != 0)
                    return Ok(Result.Ok("删除成功"));
                else
                    return Ok(Result.Err("用户名不存在"));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("foreign"))
                    return Ok(Result.Err("发布了作品或活动的用户不能删除"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPost("check")]
        public ActionResult UserCheck([FromBody]Model.UserInfo users)
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)
                    return Ok(Result.Err("用户名错误"));
                else if (result.passWord == users.passWord)
                {
                    if (result.type == "管理员")
                    {
                        result.passWord = "********";
                        return Ok(Result.Ok("管理员登录成功", result));
                    }
                    else
                        return Ok(Result.Err("只有管理员能够进入后台管理"));
                }
                else
                    return Ok(Result.Err("密码错误"));
            }
            catch(Exception ex)
            {
                return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPost("genCheck")]
        public ActionResult genUserCheck([FromBody]Model.UserInfo users)
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)
                    return Ok(Result.Err("用户名错误"));
                else if (result.passWord == users.passWord)
                {
                    result.passWord = "******";
                    return Ok(Result.Ok("登录成功", result));
                }
                else
                    return Ok(Result.Err("密码错误"));
            }
            catch(Exception ex)
            {
                return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPost("page")]
        public ActionResult getPage([FromBody] Model.Page page)
        {
            var result = DAL.UserInfo.Instance.GetPage(page);
            if (result.Count() == 0)
                return Ok(Result.Err("返回记录数为0"));
            else
                return Ok(Result.Ok(result));
        }
    }
}
