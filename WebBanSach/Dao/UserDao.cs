using WebBanSach.Data;
using WebBanSach.Models;
namespace WebBanSach.Dao
{
    public class UserDao
    {
        private dbcontext _db;
        public UserDao(dbcontext db)
        {
            _db = db;
        }
        public bool CheckUserName(string userName)
        {
            bool check = false;
            if (_db.KhachHangs.ToList().Count(x => x.HoVaTen == userName) > 1)
            {
                check = true;
            }
            return check;
        }
        public bool CheckEmail(string email)
        {
            return _db.KhachHangs.Count(x => x.Email == email) > 0;
        }
        public long Insert(KhachHang entity)
        {
            _db.KhachHangs.Add(entity);
            _db.SaveChanges();
            return entity.TrangThai;
        }
    }
}
