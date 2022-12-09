﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Utils
{
    public static class ConstMessage
    {
        //Rule : Start by H + 3 Character + number
        //public enum ErrorCode
        //{
        //    HACC001,
        //    HACC002,
        //    HACC003,
        //    HACC004,

        //}
        public static Dictionary<string, string> DicMessage()
        {
            Dictionary<string, string> _dictionary = new Dictionary<string, string>();
            _dictionary.Add("001", "Có lỗi trong quá trình thêm mới");
            _dictionary.Add("002", "Có lỗi trong quá trình update");
            _dictionary.Add("003", "Có lỗi trong quá trình xóa");
            _dictionary.Add("004", "Không tồn tại dữ liệu");
            _dictionary.Add("005", "Lỗi không xác định liên hệ admin");
            _dictionary.Add("006", "Thêm mới thành công");
            _dictionary.Add("007", "Cập nhật thành công");
            _dictionary.Add("008", "Xóa thành công");
            _dictionary.Add("009", "Dữ liệu đã tồn tại");
            _dictionary.Add("ACC006", "Chưa nhập email và password");
            _dictionary.Add("ACC007", "Mật khẩu không đúng!");
            _dictionary.Add("ACC008", "Tài khoản không tồn tại");
            _dictionary.Add("ACC009", "Tài khoản đã tồn tại");
            _dictionary.Add("ACC010", "Email hoặc số điện thoại không đúng định dạng");
            _dictionary.Add("ACC011", "Tài khoản bị khóa");
            _dictionary.Add("ACC012", "Xác nhận không thành công");
            _dictionary.Add("ACC013", "Tài khoản chưa xác thực");
            _dictionary.Add("ACC014", "Không có file avatar upload");
            _dictionary.Add("ACC015", "Mật khẩu cũ không chính xác");
            _dictionary.Add("ACC016", "Mật khẩu không trùng nhau");
            _dictionary.Add("BRAND001", "Email không đúng định dạng");
            _dictionary.Add("BRAND002", "Số điện thoại không đúng định dạng");
            _dictionary.Add("BRAND003", "Đã tồn tại nhà cung cấp");
            _dictionary.Add("BRAND004", "Không tồn tại nhà cung cấp");
            _dictionary.Add("PROD001", "Không có quyền chỉnh sửa sản phẩm");
            return _dictionary;
        }

        public static string GetMsgConst(string code)
        {
            return DicMessage()[code] == null ? "" : DicMessage()[code];
        }

    }
}
