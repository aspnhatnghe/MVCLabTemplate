CREATE DATABASE NhanSu;

USE NhanSu;

CREATE TABLE PhongBan
(
	MaPB varchar(5) not null,
	TenPB nvarchar(30) not null,
	constraint PhongBan_PK primary key(MaPB),
	constraint TenPK_Check unique (TenPB)
);

CREATE TABLE NhanVien
(
	MaNV varchar(7) not null,
	HoTen nvarchar(50) not null,
	GioiTinh nvarchar(4),
	NgaySinh datetime,
	Luong	float,
	MaPB varchar(5),
	constraint NhanVien_PK primary key(MaNV),
	constraint NhanVien_MaPB_FK foreign key(MaPB) references PhongBan(MaPB),
	constraint GioiTinh_Check check (GioiTinh in (N'Nam', N'Nữ'))
);
--Trường hợp quên tạo check cho Giới Tính
--alter table NhanVien add constraint GioiTinh_Check check (GioiTinh in (N'Nam', N'Nữ'))

--Chèn vào bảng PhongBan
insert into PhongBan(MaPB, TenPB) values('NS', N'Nhân sự');
insert into PhongBan(MaPB, TenPB) values('KT', 'Kế toán');
insert into PhongBan(MaPB, TenPB) values('TH', N'Tin học');
insert into PhongBan(MaPB, TenPB) values('KD', N'Kinh doanh tiếp thị');
insert into PhongBan(MaPB, TenPB) values('NC', N'Nghiên cứu phát triển');

--Chèn vào bảng NhanVien
insert into NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, Luong, MaPB)
	values('SV01', N'Phạm Văn Hai', N'Nam', '1/1/1990', 4000000,'NS');
insert into NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, Luong, MaPB)
	values('SV02', N'Lê Thị Hồng Gấm', N'Nữ', '2/2/1980', 13000000,'KT');
insert into NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, Luong, MaPB)
	values('SV03', N'Nguyễn Thị Minh Khai', N'Nữ', '3/3/1970', 20000000,'NS');
insert into NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, Luong, MaPB)
	values('SV04', N'Nguyễn Văn Trổi', N'Nam', '4/4/1960', 7000000,'TH');
insert into NhanVien(MaNV, HoTen, GioiTinh, NgaySinh, Luong, MaPB)
	values('SV05', N'Hoàng Văn Thụ', N'Nam', '5/5/1995', 14000000,'KD');
	
--Câu 1
SELECT A.*
FROM NhanVien as A INNER JOIN PhongBan as B ON A.MaPB = B.MaPB
WHERE GioiTinh = N'Nữ' AND TenPB = N'Nhân sự'

--Câu 2
select * from NhanVien
where HoTen like N'Nguyễn%'

--Câu 3
select * from NhanVien
where Luong between 5000000 and 10000000

--Câu 4
update NhanVien
set Luong = Luong * 1.05
where YEAR(NgaySinh) < 1970

--Câu 5
delete NhanVien where MaNV in ('SV01','SV02','SV03')

--Câu 6
select MaNV,HoTen, YEAR(NgaySinh) as NamSinh from NhanVien
order by YEAR(NgaySinh) asc

--Câu 7
select a.MaNV,a.HoTen,b.TenPB from NhanVien a
inner join PhongBan b on a.MaPB=b.MaPB

--Câu 8
select b.TenPB ,COUNT(a.MaNV) as Soluong, 
		MIN(year(getdate()) - year(a.NgaySinh)) as TuoiTreNhat,
		AVG(a.Luong) as LuongTB
from NhanVien a right join PhongBan b on a.MaPB=b.MaPB
group by a.MaPB,b.TenPB