using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebBanSach.Data;
using WebBanSach.Models.Extend;

namespace WebBanSach.Controllers
{
    public class LoginController : Controller
    {
        public IConfiguration _configuration;
        private dbcontext _db;
        public LoginController(dbcontext dbcontext, IConfiguration configuration)
        {
            _db = dbcontext;
            _configuration = configuration;
        }
        public IActionResult Login(bool nv)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.nv = nv;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin, bool nv)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.nv = nv;
            if (ModelState.IsValid)
            {
                if (!nv)
                {
                    if (_db.KhachHangs.ToList().Exists(c => c.Email == userLogin.User && c.MatKhau == userLogin.Password))
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, userLogin.User),
                            new Claim(ClaimTypes.Role, "KhachHang")
                        };
                        var identity = new ClaimsIdentity(claims, "MyCookie");  
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync("MyCookie", principal);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại");
                    }
                }
                else
                {
                    if (_db.NhanViens.ToList().Exists(c => c.Email == userLogin.User && c.MatKhau == userLogin.Password))
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, userLogin.User),
                            new Claim("Name", userLogin.User)
                        };
                        if (_db.NhanViens.FirstOrDefault(c => c.Email == userLogin.User).Quyen)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "NhanVien"));
                        }
                        var identity = new ClaimsIdentity(claims, "MyCookie");
                        var c = new List<Claim>()
                        {
                             new Claim("Sth", "Sth")
                        };
                        var i2 = new ClaimsIdentity(c, "MyCookie");
                        var lst = new List<ClaimsIdentity>() { identity, i2 };
                        ClaimsPrincipal principal = new ClaimsPrincipal(lst);
                        var auth = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        await HttpContext.SignInAsync("MyCookie", principal, auth);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại");
                    }
                }
            }
            return View(userLogin);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookie");
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult ChangePw(bool nv)
        {
            ViewBag.nv = nv;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePw(bool nv, string p1, Password p2, string p3)
        {
            ViewBag.nv = nv;
            dynamic a;
            if (nv)
            {
                a = _db.NhanViens.ToList().FirstOrDefault(c => c.Email == HttpContext.User.Identity.Name);

            }
            else
            {
                a = _db.KhachHangs.ToList().FirstOrDefault(c => c.Email == HttpContext.User.Identity.Name);
            }
            if (ModelState.IsValid && a.MatKhau == p1 && p2.pw == p3 && p2 != null)
            {
                a.MatKhau = p2.pw;
                _db.Update(a);
                _db.SaveChanges();
                return Content("Done");
            }
            else
            {
                ModelState.AddModelError("Error", "Something wrong");
            }
            return View();
        }
    }
}
