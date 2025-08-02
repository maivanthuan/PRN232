html {
    font - size: 14px;
}

@media(min - width: 768px) {
  html {
        font - size: 16px;
    }
}

.btn: focus, .btn: active: focus, .btn - link.nav - link: focus, .form - control: focus, .form - check - input:focus {
    box - shadow: 0 0 0 0.1rem white, 0 0 0 0.25rem #258cfb;
}

html {
    position: relative;
    min - height: 100 %;
}

body {
    margin - bottom: 60px;
}
.navbar - brand {
    font - size: 1.8rem!important; /* Tăng kích thước chữ, !important để ghi đè Bootstrap */
    font - weight: bold!important; /* In đậm chữ */
    color: #004d40!important; /* Có thể đổi màu cho nổi bật hơn, ví dụ màu xanh sân cỏ */
}

    .navbar - brand i {
    font - size: 1.5rem!important; /* Điều chỉnh kích thước icon nếu cần */
    margin - right: 5px; /* Khoảng cách giữa icon và chữ */
}

/* Làm cho các liên kết trên Navbar (Đặt sân, Lịch sử, Profile, Đăng xuất/nhập, Đăng ký) to hơn và đậm hơn */
.navbar - nav.nav - link {
    font - size: 1.1rem!important; /* Tăng kích thước chữ của các liên kết */
    font - weight: 600!important; /* In đậm hơn (600 hoặc 700 tùy mức độ) */
    padding - left: 15px!important; /* Tăng khoảng cách giữa các nút */
    padding - right: 15px!important; /* Tăng khoảng cách giữa các nút */
    color: #333!important; /* Đảm bảo màu chữ rõ ràng */
    transition: color 0.3s ease; /* Thêm hiệu ứng chuyển động mượt mà khi di chuột */
}

    /* Đổi màu khi di chuột vào các nút */
    .navbar - nav.nav - link:hover {
    color: #007bff!important; /* Đổi màu khi hover, ví dụ màu xanh dương của Bootstrap */
}

/* Tùy chỉnh riêng cho nút Đăng xuất (nếu là form button) */
.navbar - nav.form - inline.btn - link {
    font - size: 1.1rem!important;
    font - weight: 600!important;
    color: #333!important;
    padding - left: 15px!important;
    padding - right: 15px!important;
    /* Loại bỏ gạch chân mặc định của btn-link */
    text - decoration: none;
}

    .navbar - nav.form - inline.btn - link:hover {
    color: #dc3545!important; /* Màu đỏ cho Đăng xuất khi hover */
    text - decoration: underline; /* Thêm gạch chân khi hover để rõ hơn là liên kết */
}