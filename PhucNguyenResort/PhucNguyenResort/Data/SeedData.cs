using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Data;

public static class SeedData
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = @"System Admin" },
            new Role { RoleId = 2, RoleName = @"Business Manager" },
            new Role { RoleId = 3, RoleName = @"Customer" },
            new Role { RoleId = 4, RoleName = @"Receptionist" },
            new Role { RoleId = 5, RoleName = @"F&B Staff" },
            new Role { RoleId = 6, RoleName = @"Kitchen Staff" },
            new Role { RoleId = 7, RoleName = @"Tour Guide" },
            new Role { RoleId = 8, RoleName = @"Housekeeping Staff" },
            new Role { RoleId = 9, RoleName = @"Maintenance Staff" }
        );

        //Password@123
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, FullName = @"Nguyen Van An", Email = @"user01@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEAPfHpZqsicuzNWwEz2A+27fHA4T7jfsm+ZJVi/FMknonhnS15owgiKdE/xqvZZHvA==", PhoneNumber = @"0900000001", RoleId = 1, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 8, 30, 0) },
            new User { UserId = 2, FullName = @"Tran Thi Binh", Email = @"user02@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEMqJfvDTu1nRZ1tWnVSdC2TxoksCbzZBG0Z5+hSqJBH6ubpqvEb9ytkmbnjrBlCJNQ==", PhoneNumber = @"0900000002", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2026, 1, 2, 8, 30, 0) },
            new User { UserId = 3, FullName = @"Le Minh Chau", Email = @"user03@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEL7OmuUdZtRjIWsMK4GpOru5NwFTcgkWIMT0XsKM12jVPo5pBAI3n0mPC42TwT4gQA==", PhoneNumber = @"0900000003", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 1, 3, 8, 30, 0) },
            new User { UserId = 4, FullName = @"Pham Quoc Dung", Email = @"user04@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEPpO/sBlLtEog0z9muGuZ9dJU2By/al3/9rnCHHgIHGaUn3cCQxp8+rOHVNo771raA==", PhoneNumber = @"0900000004", RoleId = 4, IsActive = true, CreatedAt = new DateTime(2026, 1, 4, 8, 30, 0) },
            new User { UserId = 5, FullName = @"Hoang Thu Ha", Email = @"user05@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAELLhWKBA7UEbgmc/qhu3mxKfNo1Y1UI6zVG4H6XqUfwyhBZIvWkZmDZrkXQK/KaAXQ==", PhoneNumber = @"0900000005", RoleId = 5, IsActive = true, CreatedAt = new DateTime(2026, 2, 5, 8, 30, 0) },
            new User { UserId = 6, FullName = @"Vo Tuan Kiet", Email = @"user06@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEMos2lHKwL8xBwVjMAzlIWjtRXvVolAJDK4aTarDmRFhEgqc2mtMqB16Gvi4Y7hd7g==", PhoneNumber = @"0900000006", RoleId = 6, IsActive = true, CreatedAt = new DateTime(2026, 2, 6, 8, 30, 0) },
            new User { UserId = 7, FullName = @"Dang Ngoc Linh", Email = @"user07@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEENQal0elXTjB9MuTiDqZw+Ju1XFx+c6PHLzlv+vB49TGulgOELiwxVLzBLR3zWcsA==", PhoneNumber = @"0900000007", RoleId = 7, IsActive = true, CreatedAt = new DateTime(2026, 2, 7, 8, 30, 0) },
            new User { UserId = 8, FullName = @"Bui Gia Minh", Email = @"user08@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEJT25/CCK5rCrhV6iqZaRiccHs5/2PjxovAkY33vQQnePVOlNtFItn9Ej8hl8/GaVg==", PhoneNumber = @"0900000008", RoleId = 8, IsActive = true, CreatedAt = new DateTime(2026, 2, 8, 8, 30, 0) },
            new User { UserId = 9, FullName = @"Do Phuong Nam", Email = @"user09@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEP/y/42LBSamtT9RhWTarQ8Arul0XOAu1F1fy0D9x/QhZvsyBZsXcw4BK2FianAmlQ==", PhoneNumber = @"0900000009", RoleId = 9, IsActive = false, CreatedAt = new DateTime(2026, 3, 9, 8, 30, 0) },
            new User { UserId = 10, FullName = @"Ngo Thanh Oanh", Email = @"user10@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEHhfJbFM/03T1KAicxYcV1bTBoKZiD9CS841SnAOmTfifLMab/d2Ler9troq3w/uxQ==", PhoneNumber = @"0900000010", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 3, 10, 8, 30, 0) },
            new User { UserId = 11, FullName = @"Huynh Duc Phat", Email = @"user11@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEPaitqXlGlPfjdR10IkWztfw5bkaSETVCtiz1raV6zzlXBjpPzIMqeY4bfviE6e3Wg==", PhoneNumber = @"0900000011", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 3, 11, 8, 30, 0) },
            new User { UserId = 12, FullName = @"Phan My Quyen", Email = @"user12@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEDGl1/D1FrIqtwdXMp4MHKTcKLhrgp0BM7tRkmCJKU4CLs78GCzRBm6BNMw2g5wsMg==", PhoneNumber = @"0900000012", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 3, 12, 8, 30, 0) },
            new User { UserId = 13, FullName = @"Vu Khanh Son", Email = @"user13@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEOUNXbZx/sSqOf8XhutyA8c0i1jpPfwprTXZMpZ+ddTO0eZ6DeGC6SappK1tnuLQtQ==", PhoneNumber = @"0900000013", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 4, 13, 8, 30, 0) },
            new User { UserId = 14, FullName = @"Truong Hai Tam", Email = @"user14@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEJJGfYgjY5DIU1LvrA7IqfQ9N0dG1VIKmKczDlrjsfeYlLZMV3uGZM3KB8t7jPVC0g==", PhoneNumber = @"0900000014", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 4, 14, 8, 30, 0) },
            new User { UserId = 15, FullName = @"Ly Bao Uyen", Email = @"user15@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAELC/3eh2uKrbHlslXEaKmDbBCqjLTpF7KJkmuTokz/4ZZtRftNGdc1r18mlfAWJ/zg==", PhoneNumber = @"0900000015", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 4, 15, 8, 30, 0) },
            new User { UserId = 16, FullName = @"Mai Hoai Viet", Email = @"user16@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEK8Tu2bzYxEFkVglibJ2FMF/v+tZLjtO8PlvonraPzVigrXUv+ezf6txnN9tzmUKRg==", PhoneNumber = @"0900000016", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 4, 16, 8, 30, 0) },
            new User { UserId = 17, FullName = @"Dinh Nhat Vy", Email = @"user17@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEEu1vtHH1DPKJyipEF9CLiGKB5nlEfAALCHhcLHUl6Gj7930Z5fC8qFxmwbLTbFcnA==", PhoneNumber = @"0900000017", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 5, 17, 8, 30, 0) },
            new User { UserId = 18, FullName = @"Cao Tien Dat", Email = @"user18@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEBfvYhV6RZ4eaYPLilejiLRhlW1+Yc+NqGZ5EBekRqifvfY6ys/tZUV/GwRQrpr9sw==", PhoneNumber = @"0900000018", RoleId = 3, IsActive = false, CreatedAt = new DateTime(2026, 5, 18, 8, 30, 0) },
            new User { UserId = 19, FullName = @"Ta Minh Anh", Email = @"user19@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEGtWiO8pUkr97O2XfZppC3E24zWNciBmmNkqv/H5mHZXU64zaC04ozH9WwBorN8/WA==", PhoneNumber = @"0900000019", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 5, 19, 8, 30, 0) },
            new User { UserId = 20, FullName = @"Lam Gia Bao", Email = @"user20@phucnguyenresort.test", PasswordHash = @"AQAAAAIAAYagAAAAEOWoOu1dgyXSeUHjo7zAm5WWbYnrLWNWkMpjSUCTsJ3Vlznm0hcvxtyPQ/7cgwZNng==", PhoneNumber = @"0900000020", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2026, 5, 20, 8, 30, 0) }
        );

        modelBuilder.Entity<UserProfile>().HasData(
            new UserProfile { ProfileId = 1, UserId = 1, DateOfBirth = new DateTime(1986, 1, 2, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 81, 66, 84, 84, 1 }, PassportImage = @"/uploads/passports/user-01.jpg", Address = @"1 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 2, UserId = 2, DateOfBirth = new DateTime(1987, 2, 4, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 82, 67, 85, 85, 2 }, PassportImage = @"/uploads/passports/user-02.jpg", Address = @"2 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 3, UserId = 3, DateOfBirth = new DateTime(1988, 3, 6, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 83, 68, 86, 86, 3 }, PassportImage = @"/uploads/passports/user-03.jpg", Address = @"3 Bien Xanh Street, Phu Quoc", ConsentGiven = false, DeletionRequested = false },
            new UserProfile { ProfileId = 4, UserId = 4, DateOfBirth = new DateTime(1989, 4, 8, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 84, 69, 87, 87, 4 }, PassportImage = @"/uploads/passports/user-04.jpg", Address = @"4 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 5, UserId = 5, DateOfBirth = new DateTime(1990, 5, 10, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 85, 70, 88, 88, 5 }, PassportImage = @"/uploads/passports/user-05.jpg", Address = @"5 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 6, UserId = 6, DateOfBirth = new DateTime(1991, 6, 12, 0, 0, 0), Nationality = @"Korean", PassportNumber = new byte[] { 86, 71, 89, 89, 6 }, PassportImage = @"/uploads/passports/user-06.jpg", Address = @"6 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 7, UserId = 7, DateOfBirth = new DateTime(1992, 7, 14, 0, 0, 0), Nationality = @"Japanese", PassportNumber = new byte[] { 87, 72, 90, 90, 7 }, PassportImage = @"/uploads/passports/user-07.jpg", Address = @"7 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 8, UserId = 8, DateOfBirth = new DateTime(1993, 8, 16, 0, 0, 0), Nationality = @"American", PassportNumber = new byte[] { 88, 73, 91, 91, 8 }, PassportImage = @"/uploads/passports/user-08.jpg", Address = @"8 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 9, UserId = 9, DateOfBirth = new DateTime(1994, 9, 18, 0, 0, 0), Nationality = @"French", PassportNumber = new byte[] { 89, 74, 92, 92, 9 }, PassportImage = @"/uploads/passports/user-09.jpg", Address = @"9 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 10, UserId = 10, DateOfBirth = new DateTime(1995, 10, 20, 0, 0, 0), Nationality = @"German", PassportNumber = new byte[] { 90, 75, 93, 93, 10 }, PassportImage = @"/uploads/passports/user-10.jpg", Address = @"10 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 11, UserId = 11, DateOfBirth = new DateTime(1996, 11, 22, 0, 0, 0), Nationality = @"Australian", PassportNumber = new byte[] { 91, 76, 94, 94, 11 }, PassportImage = @"/uploads/passports/user-11.jpg", Address = @"11 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 12, UserId = 12, DateOfBirth = new DateTime(1997, 12, 24, 0, 0, 0), Nationality = @"Singaporean", PassportNumber = new byte[] { 92, 77, 95, 95, 12 }, PassportImage = @"/uploads/passports/user-12.jpg", Address = @"12 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 13, UserId = 13, DateOfBirth = new DateTime(1998, 1, 26, 0, 0, 0), Nationality = @"Thai", PassportNumber = new byte[] { 93, 78, 96, 96, 13 }, PassportImage = @"/uploads/passports/user-13.jpg", Address = @"13 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 14, UserId = 14, DateOfBirth = new DateTime(1999, 2, 28, 0, 0, 0), Nationality = @"Malaysian", PassportNumber = new byte[] { 94, 79, 97, 97, 14 }, PassportImage = @"/uploads/passports/user-14.jpg", Address = @"14 Bien Xanh Street, Phu Quoc", ConsentGiven = false, DeletionRequested = true },
            new UserProfile { ProfileId = 15, UserId = 15, DateOfBirth = new DateTime(2000, 3, 2, 0, 0, 0), Nationality = @"British", PassportNumber = new byte[] { 95, 80, 98, 98, 15 }, PassportImage = @"/uploads/passports/user-15.jpg", Address = @"15 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 16, UserId = 16, DateOfBirth = new DateTime(2001, 4, 4, 0, 0, 0), Nationality = @"Canadian", PassportNumber = new byte[] { 96, 81, 99, 99, 16 }, PassportImage = @"/uploads/passports/user-16.jpg", Address = @"16 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 17, UserId = 17, DateOfBirth = new DateTime(2002, 5, 6, 0, 0, 0), Nationality = @"Indian", PassportNumber = new byte[] { 97, 82, 100, 100, 17 }, PassportImage = @"/uploads/passports/user-17.jpg", Address = @"17 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 18, UserId = 18, DateOfBirth = new DateTime(2003, 6, 8, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 98, 83, 101, 101, 18 }, PassportImage = @"/uploads/passports/user-18.jpg", Address = @"18 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 19, UserId = 19, DateOfBirth = new DateTime(2004, 7, 10, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 99, 84, 102, 102, 19 }, PassportImage = @"/uploads/passports/user-19.jpg", Address = @"19 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false },
            new UserProfile { ProfileId = 20, UserId = 20, DateOfBirth = new DateTime(1985, 8, 12, 0, 0, 0), Nationality = @"Vietnamese", PassportNumber = new byte[] { 100, 85, 103, 103, 20 }, PassportImage = @"/uploads/passports/user-20.jpg", Address = @"20 Bien Xanh Street, Phu Quoc", ConsentGiven = true, DeletionRequested = false }
        );

        modelBuilder.Entity<RoomCategory>().HasData(
            new RoomCategory { CategoryId = 1, CategoryName = @"Deluxe Ocean View", Description = @"Phòng cao cấp hướng biển", MaxAdults = 2, MaxChildren = 1, PricePerNight = 1200000m, ImageUrl = @"/img/room/room-1.jpg" },
            new RoomCategory { CategoryId = 2, CategoryName = @"Premium Garden View", Description = @"Phòng view vườn yên tĩnh", MaxAdults = 2, MaxChildren = 1, PricePerNight = 1000000m, ImageUrl = @"/img/room/room-2.jpg" },
            new RoomCategory { CategoryId = 3, CategoryName = @"Family Suite", Description = @"Suite rộng rãi cho gia đình", MaxAdults = 4, MaxChildren = 2, PricePerNight = 2500000m, ImageUrl = @"/img/room/room-3.jpg" },
            new RoomCategory { CategoryId = 4, CategoryName = @"Presidential Villa", Description = @"Biệt thự siêu cấp sang trọng", MaxAdults = 6, MaxChildren = 4, PricePerNight = 8000000m, ImageUrl = @"/img/room/room-4.jpg" },
            new RoomCategory { CategoryId = 5, CategoryName = @"Honeymoon Villa", Description = @"Biệt thự lãng mạn cho cặp đôi", MaxAdults = 2, MaxChildren = 0, PricePerNight = 3500000m, ImageUrl = @"/images/rooms/category-08.jpg" }
        );

        modelBuilder.Entity<Room>().HasData(
            new Room { RoomId = 1, RoomNumber = @"101", CategoryId = 1, FloorNumber = 1, Status = RoomStatus.Available },
            new Room { RoomId = 2, RoomNumber = @"102", CategoryId = 1, FloorNumber = 1, Status = RoomStatus.Occupied },
            new Room { RoomId = 3, RoomNumber = @"103", CategoryId = 1, FloorNumber = 1, Status = RoomStatus.Available },
            new Room { RoomId = 4, RoomNumber = @"104", CategoryId = 1, FloorNumber = 1, Status = RoomStatus.Available },

            new Room { RoomId = 5, RoomNumber = @"201", CategoryId = 2, FloorNumber = 2, Status = RoomStatus.Available },
            new Room { RoomId = 6, RoomNumber = @"202", CategoryId = 2, FloorNumber = 2, Status = RoomStatus.Maintenance },
            new Room { RoomId = 7, RoomNumber = @"203", CategoryId = 2, FloorNumber = 2, Status = RoomStatus.Available },
            new Room { RoomId = 8, RoomNumber = @"204", CategoryId = 2, FloorNumber = 2, Status = RoomStatus.Occupied },

            new Room { RoomId = 9, RoomNumber = @"301", CategoryId = 3, FloorNumber = 3, Status = RoomStatus.Available },
            new Room { RoomId = 10, RoomNumber = @"302", CategoryId = 3, FloorNumber = 3, Status = RoomStatus.Available },
            new Room { RoomId = 11, RoomNumber = @"303", CategoryId = 3, FloorNumber = 3, Status = RoomStatus.NeedsCleaning },
            new Room { RoomId = 12, RoomNumber = @"304", CategoryId = 3, FloorNumber = 3, Status = RoomStatus.Available },

            new Room { RoomId = 13, RoomNumber = @"401", CategoryId = 4, FloorNumber = 4, Status = RoomStatus.Available },
            new Room { RoomId = 14, RoomNumber = @"402", CategoryId = 4, FloorNumber = 4, Status = RoomStatus.Occupied },
            new Room { RoomId = 15, RoomNumber = @"403", CategoryId = 4, FloorNumber = 4, Status = RoomStatus.Available },
            new Room { RoomId = 16, RoomNumber = @"404", CategoryId = 4, FloorNumber = 4, Status = RoomStatus.Available },

            new Room { RoomId = 17, RoomNumber = @"501", CategoryId = 5, FloorNumber = 5, Status = RoomStatus.Available },
            new Room { RoomId = 18, RoomNumber = @"502", CategoryId = 5, FloorNumber = 5, Status = RoomStatus.Available },
            new Room { RoomId = 19, RoomNumber = @"503", CategoryId = 5, FloorNumber = 5, Status = RoomStatus.Available },
            new Room { RoomId = 20, RoomNumber = @"504", CategoryId = 5, FloorNumber = 5, Status = RoomStatus.NeedsCleaning }
        );

        modelBuilder.Entity<Booking>().HasData(
            new Booking { BookingId = 1, UserId = 2, CheckInDate = DateTime.Now.AddDays(-10), CheckOutDate = DateTime.Now.AddDays(-8), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 1200000m, TotalAmount = 2400000m, CreatedAt = DateTime.Now.AddDays(-15) },
            new Booking { BookingId = 2, UserId = 2, CheckInDate = DateTime.Now.AddDays(5), CheckOutDate = DateTime.Now.AddDays(8), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.Reserved, DepositAmount = 1500000m, TotalAmount = 3000000m, CreatedAt = DateTime.Now.AddDays(-2) },
            new Booking { BookingId = 3, UserId = 3, CheckInDate = DateTime.Now.AddDays(10), CheckOutDate = DateTime.Now.AddDays(12), NumberOfAdults = 4, NumberOfChildren = 0, BookingStatus = BookingStatus.PendingPayment, DepositAmount = 2500000m, TotalAmount = 5000000m, CreatedAt = DateTime.Now.AddDays(-1) },
            new Booking { BookingId = 4, UserId = 3, CheckInDate = DateTime.Now.AddDays(-5), CheckOutDate = DateTime.Now.AddDays(-3), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.Cancelled, DepositAmount = 1000000m, TotalAmount = 2000000m, CreatedAt = DateTime.Now.AddDays(-10) },
            new Booking { BookingId = 5, UserId = 2, CheckInDate = DateTime.Now.AddDays(1), CheckOutDate = DateTime.Now.AddDays(4), NumberOfAdults = 6, NumberOfChildren = 0, BookingStatus = BookingStatus.Reserved, DepositAmount = 12000000m, TotalAmount = 24000000m, CreatedAt = DateTime.Now.AddDays(-5) },
            new Booking { BookingId = 6, UserId = 2, CheckInDate = DateTime.Now.AddMonths(-1), CheckOutDate = DateTime.Now.AddMonths(-1).AddDays(2), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 1200000m, TotalAmount = 2400000m, CreatedAt = DateTime.Now.AddMonths(-1).AddDays(-5) },
            new Booking { BookingId = 7, UserId = 3, CheckInDate = DateTime.Now.AddMonths(-2), CheckOutDate = DateTime.Now.AddMonths(-2).AddDays(3), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 1500000m, TotalAmount = 3000000m, CreatedAt = DateTime.Now.AddMonths(-2).AddDays(-10) },
            new Booking { BookingId = 8, UserId = 3, CheckInDate = DateTime.Now.AddDays(-20), CheckOutDate = DateTime.Now.AddDays(-18), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.Cancelled, DepositAmount = 1200000m, TotalAmount = 2400000m, CreatedAt = DateTime.Now.AddDays(-25) },
            new Booking { BookingId = 9, UserId = 2, CheckInDate = DateTime.Now.AddDays(-15), CheckOutDate = DateTime.Now.AddDays(-10), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 3000000m, TotalAmount = 6000000m, CreatedAt = DateTime.Now.AddDays(-20) },
            new Booking { BookingId = 10, UserId = 3, CheckInDate = DateTime.Now.AddDays(20), CheckOutDate = DateTime.Now.AddDays(22), NumberOfAdults = 5, NumberOfChildren = 0, BookingStatus = BookingStatus.Reserved, DepositAmount = 8000000m, TotalAmount = 16000000m, CreatedAt = DateTime.Now.AddDays(-1) },
            new Booking { BookingId = 11, UserId = 2, CheckInDate = DateTime.Now.AddDays(15), CheckOutDate = DateTime.Now.AddDays(18), NumberOfAdults = 6, NumberOfChildren = 0, BookingStatus = BookingStatus.PendingPayment, DepositAmount = 12000000m, TotalAmount = 24000000m, CreatedAt = DateTime.Now.AddDays(0) },
            new Booking { BookingId = 12, UserId = 3, CheckInDate = DateTime.Now.AddMonths(-4), CheckOutDate = DateTime.Now.AddMonths(-4).AddDays(5), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 3000000m, TotalAmount = 6000000m, CreatedAt = DateTime.Now.AddMonths(-4).AddDays(-10) },
            new Booking { BookingId = 13, UserId = 3, CheckInDate = DateTime.Now.AddMonths(-5), CheckOutDate = DateTime.Now.AddMonths(-5).AddDays(1), NumberOfAdults = 1, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 600000m, TotalAmount = 1200000m, CreatedAt = DateTime.Now.AddMonths(-5).AddDays(-3) },
            new Booking { BookingId = 14, UserId = 14, CheckInDate = new DateTime(2026, 6, 15, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 17, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.Cancelled, DepositAmount = 1200000.00m, TotalAmount = 5300000.00m, CreatedAt = new DateTime(2026, 5, 15, 9, 0, 0) },
            new Booking { BookingId = 15, UserId = 15, CheckInDate = new DateTime(2026, 6, 16, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 18, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.NoShow, DepositAmount = 1250000.00m, TotalAmount = 5550000.00m, CreatedAt = new DateTime(2026, 5, 16, 9, 0, 0) },
            new Booking { BookingId = 16, UserId = 16, CheckInDate = new DateTime(2026, 6, 17, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 19, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.Reserved, DepositAmount = 1300000.00m, TotalAmount = 5800000.00m, CreatedAt = new DateTime(2026, 5, 17, 9, 0, 0) },
            new Booking { BookingId = 17, UserId = 17, CheckInDate = new DateTime(2026, 6, 18, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 20, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedIn, DepositAmount = 1350000.00m, TotalAmount = 6050000.00m, CreatedAt = new DateTime(2026, 5, 18, 9, 0, 0) },
            new Booking { BookingId = 18, UserId = 18, CheckInDate = new DateTime(2026, 6, 19, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 21, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.CheckedOut, DepositAmount = 1400000.00m, TotalAmount = 6300000.00m, CreatedAt = new DateTime(2026, 5, 19, 9, 0, 0) },
            new Booking { BookingId = 19, UserId = 19, CheckInDate = new DateTime(2026, 6, 20, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 22, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.Cancelled, DepositAmount = 1450000.00m, TotalAmount = 6550000.00m, CreatedAt = new DateTime(2026, 5, 20, 9, 0, 0) },
            new Booking { BookingId = 20, UserId = 20, CheckInDate = new DateTime(2026, 6, 21, 0, 0, 0), CheckOutDate = new DateTime(2026, 6, 23, 0, 0, 0), NumberOfAdults = 2, NumberOfChildren = 0, BookingStatus = BookingStatus.NoShow, DepositAmount = 1500000.00m, TotalAmount = 6800000.00m, CreatedAt = new DateTime(2026, 5, 21, 9, 0, 0) }
        );

        modelBuilder.Entity<BookingGuest>().HasData(
            new BookingGuest { BookingGuestId = 1, BookingId = 1, FullName = @"Guest Companion 01", DateOfBirth = new DateTime(1991, 4, 7, 0, 0, 0), PassportNumber = @"P20260001", Nationality = @"Korean" },
            new BookingGuest { BookingGuestId = 2, BookingId = 2, FullName = @"Guest Companion 02", DateOfBirth = new DateTime(1992, 5, 8, 0, 0, 0), PassportNumber = @"P20260002", Nationality = @"Japanese" },
            new BookingGuest { BookingGuestId = 3, BookingId = 3, FullName = @"Guest Companion 03", DateOfBirth = new DateTime(1993, 6, 9, 0, 0, 0), PassportNumber = @"P20260003", Nationality = @"American" },
            new BookingGuest { BookingGuestId = 4, BookingId = 4, FullName = @"Guest Companion 04", DateOfBirth = new DateTime(1994, 7, 10, 0, 0, 0), PassportNumber = @"P20260004", Nationality = @"French" },
            new BookingGuest { BookingGuestId = 5, BookingId = 5, FullName = @"Guest Companion 05", DateOfBirth = new DateTime(1995, 8, 11, 0, 0, 0), PassportNumber = @"P20260005", Nationality = @"German" },
            new BookingGuest { BookingGuestId = 6, BookingId = 6, FullName = @"Guest Companion 06", DateOfBirth = new DateTime(1996, 9, 12, 0, 0, 0), PassportNumber = @"P20260006", Nationality = @"Australian" },
            new BookingGuest { BookingGuestId = 7, BookingId = 7, FullName = @"Guest Companion 07", DateOfBirth = new DateTime(1997, 10, 13, 0, 0, 0), PassportNumber = @"P20260007", Nationality = @"Singaporean" },
            new BookingGuest { BookingGuestId = 8, BookingId = 8, FullName = @"Guest Companion 08", DateOfBirth = new DateTime(1998, 11, 14, 0, 0, 0), PassportNumber = @"P20260008", Nationality = @"Thai" },
            new BookingGuest { BookingGuestId = 9, BookingId = 9, FullName = @"Guest Companion 09", DateOfBirth = new DateTime(1999, 12, 15, 0, 0, 0), PassportNumber = @"P20260009", Nationality = @"Malaysian" },
            new BookingGuest { BookingGuestId = 10, BookingId = 10, FullName = @"Guest Companion 10", DateOfBirth = new DateTime(2000, 1, 16, 0, 0, 0), PassportNumber = @"P20260010", Nationality = @"British" },
            new BookingGuest { BookingGuestId = 11, BookingId = 11, FullName = @"Guest Companion 11", DateOfBirth = new DateTime(2001, 2, 17, 0, 0, 0), PassportNumber = @"P20260011", Nationality = @"Canadian" },
            new BookingGuest { BookingGuestId = 12, BookingId = 12, FullName = @"Guest Companion 12", DateOfBirth = new DateTime(2002, 3, 18, 0, 0, 0), PassportNumber = @"P20260012", Nationality = @"Indian" },
            new BookingGuest { BookingGuestId = 13, BookingId = 13, FullName = @"Guest Companion 13", DateOfBirth = new DateTime(2003, 4, 19, 0, 0, 0), PassportNumber = @"P20260013", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 14, BookingId = 14, FullName = @"Guest Companion 14", DateOfBirth = new DateTime(2004, 5, 20, 0, 0, 0), PassportNumber = @"P20260014", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 15, BookingId = 15, FullName = @"Guest Companion 15", DateOfBirth = new DateTime(1990, 6, 21, 0, 0, 0), PassportNumber = @"P20260015", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 16, BookingId = 16, FullName = @"Guest Companion 16", DateOfBirth = new DateTime(1991, 7, 22, 0, 0, 0), PassportNumber = @"P20260016", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 17, BookingId = 17, FullName = @"Guest Companion 17", DateOfBirth = new DateTime(1992, 8, 23, 0, 0, 0), PassportNumber = @"P20260017", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 18, BookingId = 18, FullName = @"Guest Companion 18", DateOfBirth = new DateTime(1993, 9, 24, 0, 0, 0), PassportNumber = @"P20260018", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 19, BookingId = 19, FullName = @"Guest Companion 19", DateOfBirth = new DateTime(1994, 10, 25, 0, 0, 0), PassportNumber = @"P20260019", Nationality = @"Vietnamese" },
            new BookingGuest { BookingGuestId = 20, BookingId = 20, FullName = @"Guest Companion 20", DateOfBirth = new DateTime(1995, 11, 26, 0, 0, 0), PassportNumber = @"P20260020", Nationality = @"Vietnamese" }
        );

        modelBuilder.Entity<BookingDetail>().HasData(
            new BookingDetail { BookingDetailId = 1, BookingId = 1, RoomCategoryId = 1, RoomId = 1, Price = 1200000m },
            new BookingDetail { BookingDetailId = 2, BookingId = 2, RoomCategoryId = 1, RoomId = 2, Price = 1200000m },
            new BookingDetail { BookingDetailId = 3, BookingId = 3, RoomCategoryId = 1, RoomId = 3, Price = 1200000m },
            new BookingDetail { BookingDetailId = 4, BookingId = 4, RoomCategoryId = 1, RoomId = 4, Price = 1200000m },
            new BookingDetail { BookingDetailId = 5, BookingId = 5, RoomCategoryId = 2, RoomId = 5, Price = 1000000m },
            new BookingDetail { BookingDetailId = 6, BookingId = 6, RoomCategoryId = 2, RoomId = 6, Price = 1000000m },
            new BookingDetail { BookingDetailId = 7, BookingId = 7, RoomCategoryId = 2, RoomId = 7, Price = 1000000m },
            new BookingDetail { BookingDetailId = 8, BookingId = 8, RoomCategoryId = 2, RoomId = 8, Price = 1000000m },
            new BookingDetail { BookingDetailId = 9, BookingId = 9, RoomCategoryId = 3, RoomId = 9, Price = 2500000m },
            new BookingDetail { BookingDetailId = 10, BookingId = 10, RoomCategoryId = 3, RoomId = 10, Price = 2500000m },
            new BookingDetail { BookingDetailId = 11, BookingId = 11, RoomCategoryId = 3, RoomId = 11, Price = 2500000m },
            new BookingDetail { BookingDetailId = 12, BookingId = 12, RoomCategoryId = 3, RoomId = 12, Price = 2500000m },
            new BookingDetail { BookingDetailId = 13, BookingId = 13, RoomCategoryId = 4, RoomId = 13, Price = 8000000m },
            new BookingDetail { BookingDetailId = 14, BookingId = 14, RoomCategoryId = 4, RoomId = 14, Price = 8000000m },
            new BookingDetail { BookingDetailId = 15, BookingId = 15, RoomCategoryId = 4, RoomId = 15, Price = 8000000m },
            new BookingDetail { BookingDetailId = 16, BookingId = 16, RoomCategoryId = 4, RoomId = 16, Price = 8000000m },
            new BookingDetail { BookingDetailId = 17, BookingId = 17, RoomCategoryId = 5, RoomId = 17, Price = 3500000m },
            new BookingDetail { BookingDetailId = 18, BookingId = 18, RoomCategoryId = 5, RoomId = 18, Price = 3500000m },
            new BookingDetail { BookingDetailId = 19, BookingId = 19, RoomCategoryId = 5, RoomId = 19, Price = 3500000m },
            new BookingDetail { BookingDetailId = 20, BookingId = 20, RoomCategoryId = 5, RoomId = 20, Price = 3500000m }
        );

        modelBuilder.Entity<MenuCategory>().HasData(
            new MenuCategory { CategoryId = 1, CategoryName = @"Breakfast" },
            new MenuCategory { CategoryId = 2, CategoryName = @"Appetizer" },
            new MenuCategory { CategoryId = 3, CategoryName = @"Soup" },
            new MenuCategory { CategoryId = 4, CategoryName = @"Salad" },
            new MenuCategory { CategoryId = 5, CategoryName = @"Seafood" },
            new MenuCategory { CategoryId = 6, CategoryName = @"Vietnamese Food" },
            new MenuCategory { CategoryId = 7, CategoryName = @"Western Food" },
            new MenuCategory { CategoryId = 8, CategoryName = @"Vegetarian" },
            new MenuCategory { CategoryId = 9, CategoryName = @"Dessert" },
            new MenuCategory { CategoryId = 10, CategoryName = @"Coffee" },
            new MenuCategory { CategoryId = 11, CategoryName = @"Tea" },
            new MenuCategory { CategoryId = 12, CategoryName = @"Juice" },
            new MenuCategory { CategoryId = 13, CategoryName = @"Cocktail" },
            new MenuCategory { CategoryId = 14, CategoryName = @"Mocktail" },
            new MenuCategory { CategoryId = 15, CategoryName = @"Kids Meal" },
            new MenuCategory { CategoryId = 16, CategoryName = @"Room Service Set" },
            new MenuCategory { CategoryId = 17, CategoryName = @"BBQ" },
            new MenuCategory { CategoryId = 18, CategoryName = @"Noodle" },
            new MenuCategory { CategoryId = 19, CategoryName = @"Rice Dish" },
            new MenuCategory { CategoryId = 20, CategoryName = @"Special Combo" }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { MenuItemId = 1, CategoryId = 1, ItemName = @"Pho Bo", Description = @"Pho Bo prepared by resort kitchen.", Price = 85000.00m, ImageUrl = @"/images/menu/item-01.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 2, CategoryId = 2, ItemName = @"Bun Cha", Description = @"Bun Cha prepared by resort kitchen.", Price = 110000.00m, ImageUrl = @"/images/menu/item-02.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 3, CategoryId = 3, ItemName = @"Spring Rolls", Description = @"Spring Rolls prepared by resort kitchen.", Price = 135000.00m, ImageUrl = @"/images/menu/item-03.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 4, CategoryId = 4, ItemName = @"Seafood Soup", Description = @"Seafood Soup prepared by resort kitchen.", Price = 160000.00m, ImageUrl = @"/images/menu/item-04.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 5, CategoryId = 5, ItemName = @"Caesar Salad", Description = @"Caesar Salad prepared by resort kitchen.", Price = 185000.00m, ImageUrl = @"/images/menu/item-05.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 6, CategoryId = 6, ItemName = @"Grilled Prawns", Description = @"Grilled Prawns prepared by resort kitchen.", Price = 210000.00m, ImageUrl = @"/images/menu/item-06.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 7, CategoryId = 7, ItemName = @"Beef Steak", Description = @"Beef Steak prepared by resort kitchen.", Price = 235000.00m, ImageUrl = @"/images/menu/item-07.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 8, CategoryId = 8, ItemName = @"Vegetable Curry", Description = @"Vegetable Curry prepared by resort kitchen.", Price = 260000.00m, ImageUrl = @"/images/menu/item-08.jpg", IsAvailable = false },
            new MenuItem { MenuItemId = 9, CategoryId = 9, ItemName = @"Chocolate Cake", Description = @"Chocolate Cake prepared by resort kitchen.", Price = 285000.00m, ImageUrl = @"/images/menu/item-09.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 10, CategoryId = 10, ItemName = @"Vietnamese Coffee", Description = @"Vietnamese Coffee prepared by resort kitchen.", Price = 310000.00m, ImageUrl = @"/images/menu/item-10.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 11, CategoryId = 11, ItemName = @"Lotus Tea", Description = @"Lotus Tea prepared by resort kitchen.", Price = 335000.00m, ImageUrl = @"/images/menu/item-11.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 12, CategoryId = 12, ItemName = @"Mango Juice", Description = @"Mango Juice prepared by resort kitchen.", Price = 360000.00m, ImageUrl = @"/images/menu/item-12.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 13, CategoryId = 13, ItemName = @"Sunset Cocktail", Description = @"Sunset Cocktail prepared by resort kitchen.", Price = 385000.00m, ImageUrl = @"/images/menu/item-13.jpg", IsAvailable = false },
            new MenuItem { MenuItemId = 14, CategoryId = 14, ItemName = @"Blue Lagoon Mocktail", Description = @"Blue Lagoon Mocktail prepared by resort kitchen.", Price = 410000.00m, ImageUrl = @"/images/menu/item-14.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 15, CategoryId = 15, ItemName = @"Kids Pasta", Description = @"Kids Pasta prepared by resort kitchen.", Price = 435000.00m, ImageUrl = @"/images/menu/item-15.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 16, CategoryId = 16, ItemName = @"Club Sandwich", Description = @"Club Sandwich prepared by resort kitchen.", Price = 460000.00m, ImageUrl = @"/images/menu/item-16.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 17, CategoryId = 17, ItemName = @"BBQ Platter", Description = @"BBQ Platter prepared by resort kitchen.", Price = 485000.00m, ImageUrl = @"/images/menu/item-17.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 18, CategoryId = 18, ItemName = @"Seafood Noodle", Description = @"Seafood Noodle prepared by resort kitchen.", Price = 510000.00m, ImageUrl = @"/images/menu/item-18.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 19, CategoryId = 19, ItemName = @"Chicken Fried Rice", Description = @"Chicken Fried Rice prepared by resort kitchen.", Price = 535000.00m, ImageUrl = @"/images/menu/item-19.jpg", IsAvailable = true },
            new MenuItem { MenuItemId = 20, CategoryId = 20, ItemName = @"Resort Combo", Description = @"Resort Combo prepared by resort kitchen.", Price = 560000.00m, ImageUrl = @"/images/menu/item-20.jpg", IsAvailable = true }
        );

        modelBuilder.Entity<DiningTable>().HasData(
            new DiningTable { TableId = 1, TableNumber = @"T01", Capacity = 4, Status = DiningTableStatus.Available },
            new DiningTable { TableId = 2, TableNumber = @"T02", Capacity = 6, Status = DiningTableStatus.Occupied },
            new DiningTable { TableId = 3, TableNumber = @"T03", Capacity = 8, Status = DiningTableStatus.Reserved },
            new DiningTable { TableId = 4, TableNumber = @"T04", Capacity = 10, Status = DiningTableStatus.Maintenance },
            new DiningTable { TableId = 5, TableNumber = @"T05", Capacity = 2, Status = DiningTableStatus.Available },
            new DiningTable { TableId = 6, TableNumber = @"T06", Capacity = 4, Status = DiningTableStatus.Occupied },
            new DiningTable { TableId = 7, TableNumber = @"T07", Capacity = 6, Status = DiningTableStatus.Reserved },
            new DiningTable { TableId = 8, TableNumber = @"T08", Capacity = 8, Status = DiningTableStatus.Maintenance },
            new DiningTable { TableId = 9, TableNumber = @"T09", Capacity = 10, Status = DiningTableStatus.Available },
            new DiningTable { TableId = 10, TableNumber = @"T10", Capacity = 2, Status = DiningTableStatus.Occupied },
            new DiningTable { TableId = 11, TableNumber = @"T11", Capacity = 4, Status = DiningTableStatus.Reserved },
            new DiningTable { TableId = 12, TableNumber = @"T12", Capacity = 6, Status = DiningTableStatus.Maintenance },
            new DiningTable { TableId = 13, TableNumber = @"T13", Capacity = 8, Status = DiningTableStatus.Available },
            new DiningTable { TableId = 14, TableNumber = @"T14", Capacity = 10, Status = DiningTableStatus.Occupied },
            new DiningTable { TableId = 15, TableNumber = @"T15", Capacity = 2, Status = DiningTableStatus.Reserved },
            new DiningTable { TableId = 16, TableNumber = @"T16", Capacity = 4, Status = DiningTableStatus.Maintenance },
            new DiningTable { TableId = 17, TableNumber = @"T17", Capacity = 6, Status = DiningTableStatus.Available },
            new DiningTable { TableId = 18, TableNumber = @"T18", Capacity = 8, Status = DiningTableStatus.Occupied },
            new DiningTable { TableId = 19, TableNumber = @"T19", Capacity = 10, Status = DiningTableStatus.Reserved },
            new DiningTable { TableId = 20, TableNumber = @"T20", Capacity = 2, Status = DiningTableStatus.Maintenance }
        );

        modelBuilder.Entity<TableReservation>().HasData(
            new TableReservation { ReservationId = 1, UserId = 1, TableId = 1, ReservationDate = new DateTime(2026, 6, 6, 0, 0, 0), ReservationTime = new TimeSpan(18, 30, 0), PartySize = 3, Status = TableReservationStatus.Pending },
            new TableReservation { ReservationId = 2, UserId = 2, TableId = 2, ReservationDate = new DateTime(2026, 6, 7, 0, 0, 0), ReservationTime = new TimeSpan(19, 0, 0), PartySize = 4, Status = TableReservationStatus.Confirmed },
            new TableReservation { ReservationId = 3, UserId = 3, TableId = 3, ReservationDate = new DateTime(2026, 6, 8, 0, 0, 0), ReservationTime = new TimeSpan(20, 30, 0), PartySize = 5, Status = TableReservationStatus.Completed },
            new TableReservation { ReservationId = 4, UserId = 4, TableId = 4, ReservationDate = new DateTime(2026, 6, 9, 0, 0, 0), ReservationTime = new TimeSpan(21, 0, 0), PartySize = 6, Status = TableReservationStatus.Cancelled },
            new TableReservation { ReservationId = 5, UserId = 5, TableId = 5, ReservationDate = new DateTime(2026, 6, 10, 0, 0, 0), ReservationTime = new TimeSpan(17, 30, 0), PartySize = 7, Status = TableReservationStatus.Pending },
            new TableReservation { ReservationId = 6, UserId = 6, TableId = 6, ReservationDate = new DateTime(2026, 6, 11, 0, 0, 0), ReservationTime = new TimeSpan(18, 0, 0), PartySize = 2, Status = TableReservationStatus.Confirmed },
            new TableReservation { ReservationId = 7, UserId = 7, TableId = 7, ReservationDate = new DateTime(2026, 6, 12, 0, 0, 0), ReservationTime = new TimeSpan(19, 30, 0), PartySize = 3, Status = TableReservationStatus.Completed },
            new TableReservation { ReservationId = 8, UserId = 8, TableId = 8, ReservationDate = new DateTime(2026, 6, 13, 0, 0, 0), ReservationTime = new TimeSpan(20, 0, 0), PartySize = 4, Status = TableReservationStatus.Cancelled },
            new TableReservation { ReservationId = 9, UserId = 9, TableId = 9, ReservationDate = new DateTime(2026, 6, 14, 0, 0, 0), ReservationTime = new TimeSpan(21, 30, 0), PartySize = 5, Status = TableReservationStatus.Pending },
            new TableReservation { ReservationId = 10, UserId = 10, TableId = 10, ReservationDate = new DateTime(2026, 6, 15, 0, 0, 0), ReservationTime = new TimeSpan(17, 0, 0), PartySize = 6, Status = TableReservationStatus.Confirmed },
            new TableReservation { ReservationId = 11, UserId = 11, TableId = 11, ReservationDate = new DateTime(2026, 6, 16, 0, 0, 0), ReservationTime = new TimeSpan(18, 30, 0), PartySize = 7, Status = TableReservationStatus.Completed },
            new TableReservation { ReservationId = 12, UserId = 12, TableId = 12, ReservationDate = new DateTime(2026, 6, 17, 0, 0, 0), ReservationTime = new TimeSpan(19, 0, 0), PartySize = 2, Status = TableReservationStatus.Cancelled },
            new TableReservation { ReservationId = 13, UserId = 13, TableId = 13, ReservationDate = new DateTime(2026, 6, 18, 0, 0, 0), ReservationTime = new TimeSpan(20, 30, 0), PartySize = 3, Status = TableReservationStatus.Pending },
            new TableReservation { ReservationId = 14, UserId = 14, TableId = 14, ReservationDate = new DateTime(2026, 6, 19, 0, 0, 0), ReservationTime = new TimeSpan(21, 0, 0), PartySize = 4, Status = TableReservationStatus.Confirmed },
            new TableReservation { ReservationId = 15, UserId = 15, TableId = 15, ReservationDate = new DateTime(2026, 6, 20, 0, 0, 0), ReservationTime = new TimeSpan(17, 30, 0), PartySize = 5, Status = TableReservationStatus.Completed },
            new TableReservation { ReservationId = 16, UserId = 16, TableId = 16, ReservationDate = new DateTime(2026, 6, 21, 0, 0, 0), ReservationTime = new TimeSpan(18, 0, 0), PartySize = 6, Status = TableReservationStatus.Cancelled },
            new TableReservation { ReservationId = 17, UserId = 17, TableId = 17, ReservationDate = new DateTime(2026, 6, 22, 0, 0, 0), ReservationTime = new TimeSpan(19, 30, 0), PartySize = 7, Status = TableReservationStatus.Pending },
            new TableReservation { ReservationId = 18, UserId = 18, TableId = 18, ReservationDate = new DateTime(2026, 6, 23, 0, 0, 0), ReservationTime = new TimeSpan(20, 0, 0), PartySize = 2, Status = TableReservationStatus.Confirmed },
            new TableReservation { ReservationId = 19, UserId = 19, TableId = 19, ReservationDate = new DateTime(2026, 6, 24, 0, 0, 0), ReservationTime = new TimeSpan(21, 30, 0), PartySize = 3, Status = TableReservationStatus.Completed },
            new TableReservation { ReservationId = 20, UserId = 20, TableId = 20, ReservationDate = new DateTime(2026, 6, 25, 0, 0, 0), ReservationTime = new TimeSpan(17, 0, 0), PartySize = 4, Status = TableReservationStatus.Cancelled }
        );

        modelBuilder.Entity<FoodOrder>().HasData(
            new FoodOrder { FoodOrderId = 1, BookingId = 1, OrderStatus = FoodOrderStatus.Pending, TotalAmount = 170000.00m, OrderedAt = new DateTime(2026, 6, 3, 13, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 2, BookingId = 2, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 330000.00m, OrderedAt = new DateTime(2026, 6, 4, 14, 15, 0), IsChargedToRoom = true, ChargedAt = new DateTime(2026, 6, 18, 10, 30, 0) },
            new FoodOrder { FoodOrderId = 3, BookingId = 3, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 540000.00m, OrderedAt = new DateTime(2026, 6, 5, 15, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 4, BookingId = 4, OrderStatus = FoodOrderStatus.Delivered, TotalAmount = 160000.00m, OrderedAt = new DateTime(2026, 6, 6, 16, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 5, BookingId = 5, OrderStatus = FoodOrderStatus.Cancelled, TotalAmount = 370000.00m, OrderedAt = new DateTime(2026, 6, 7, 17, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 6, BookingId = 6, OrderStatus = FoodOrderStatus.Pending, TotalAmount = 630000.00m, OrderedAt = new DateTime(2026, 6, 8, 18, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 7, BookingId = 7, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 940000.00m, OrderedAt = new DateTime(2026, 6, 9, 19, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 8, BookingId = 8, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 260000.00m, OrderedAt = new DateTime(2026, 6, 10, 12, 15, 0), IsChargedToRoom = true, ChargedAt = new DateTime(2026, 6, 24, 10, 30, 0) },
            new FoodOrder { FoodOrderId = 9, BookingId = 9, OrderStatus = FoodOrderStatus.Delivered, TotalAmount = 570000.00m, OrderedAt = new DateTime(2026, 6, 11, 13, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 10, BookingId = 10, OrderStatus = FoodOrderStatus.Cancelled, TotalAmount = 930000.00m, OrderedAt = new DateTime(2026, 6, 12, 14, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 11, BookingId = 11, OrderStatus = FoodOrderStatus.Pending, TotalAmount = 1340000.00m, OrderedAt = new DateTime(2026, 6, 13, 15, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 12, BookingId = 12, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 360000.00m, OrderedAt = new DateTime(2026, 6, 14, 16, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 13, BookingId = 13, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 770000.00m, OrderedAt = new DateTime(2026, 6, 15, 17, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 14, BookingId = 14, OrderStatus = FoodOrderStatus.Delivered, TotalAmount = 1230000.00m, OrderedAt = new DateTime(2026, 6, 16, 18, 15, 0), IsChargedToRoom = true, ChargedAt = new DateTime(2026, 6, 30, 10, 30, 0) },
            new FoodOrder { FoodOrderId = 15, BookingId = 15, OrderStatus = FoodOrderStatus.Cancelled, TotalAmount = 1740000.00m, OrderedAt = new DateTime(2026, 6, 17, 19, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 16, BookingId = 16, OrderStatus = FoodOrderStatus.Pending, TotalAmount = 460000.00m, OrderedAt = new DateTime(2026, 6, 18, 12, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 17, BookingId = 17, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 970000.00m, OrderedAt = new DateTime(2026, 6, 19, 13, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 18, BookingId = 18, OrderStatus = FoodOrderStatus.Cooking, TotalAmount = 1530000.00m, OrderedAt = new DateTime(2026, 6, 20, 14, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 19, BookingId = 19, OrderStatus = FoodOrderStatus.Delivered, TotalAmount = 2140000.00m, OrderedAt = new DateTime(2026, 6, 21, 15, 15, 0), IsChargedToRoom = false, ChargedAt = null },
            new FoodOrder { FoodOrderId = 20, BookingId = 20, OrderStatus = FoodOrderStatus.Cancelled, TotalAmount = 560000.00m, OrderedAt = new DateTime(2026, 6, 22, 16, 15, 0), IsChargedToRoom = true, ChargedAt = new DateTime(2026, 7, 6, 10, 30, 0) }
        );

        modelBuilder.Entity<FoodOrderItem>().HasData(
            new FoodOrderItem { OrderItemId = 1, FoodOrderId = 1, MenuItemId = 1, Quantity = 2, UnitPrice = 85000.00m },
            new FoodOrderItem { OrderItemId = 2, FoodOrderId = 2, MenuItemId = 2, Quantity = 3, UnitPrice = 110000.00m },
            new FoodOrderItem { OrderItemId = 3, FoodOrderId = 3, MenuItemId = 3, Quantity = 4, UnitPrice = 135000.00m },
            new FoodOrderItem { OrderItemId = 4, FoodOrderId = 4, MenuItemId = 4, Quantity = 1, UnitPrice = 160000.00m },
            new FoodOrderItem { OrderItemId = 5, FoodOrderId = 5, MenuItemId = 5, Quantity = 2, UnitPrice = 185000.00m },
            new FoodOrderItem { OrderItemId = 6, FoodOrderId = 6, MenuItemId = 6, Quantity = 3, UnitPrice = 210000.00m },
            new FoodOrderItem { OrderItemId = 7, FoodOrderId = 7, MenuItemId = 7, Quantity = 4, UnitPrice = 235000.00m },
            new FoodOrderItem { OrderItemId = 8, FoodOrderId = 8, MenuItemId = 8, Quantity = 1, UnitPrice = 260000.00m },
            new FoodOrderItem { OrderItemId = 9, FoodOrderId = 9, MenuItemId = 9, Quantity = 2, UnitPrice = 285000.00m },
            new FoodOrderItem { OrderItemId = 10, FoodOrderId = 10, MenuItemId = 10, Quantity = 3, UnitPrice = 310000.00m },
            new FoodOrderItem { OrderItemId = 11, FoodOrderId = 11, MenuItemId = 11, Quantity = 4, UnitPrice = 335000.00m },
            new FoodOrderItem { OrderItemId = 12, FoodOrderId = 12, MenuItemId = 12, Quantity = 1, UnitPrice = 360000.00m },
            new FoodOrderItem { OrderItemId = 13, FoodOrderId = 13, MenuItemId = 13, Quantity = 2, UnitPrice = 385000.00m },
            new FoodOrderItem { OrderItemId = 14, FoodOrderId = 14, MenuItemId = 14, Quantity = 3, UnitPrice = 410000.00m },
            new FoodOrderItem { OrderItemId = 15, FoodOrderId = 15, MenuItemId = 15, Quantity = 4, UnitPrice = 435000.00m },
            new FoodOrderItem { OrderItemId = 16, FoodOrderId = 16, MenuItemId = 16, Quantity = 1, UnitPrice = 460000.00m },
            new FoodOrderItem { OrderItemId = 17, FoodOrderId = 17, MenuItemId = 17, Quantity = 2, UnitPrice = 485000.00m },
            new FoodOrderItem { OrderItemId = 18, FoodOrderId = 18, MenuItemId = 18, Quantity = 3, UnitPrice = 510000.00m },
            new FoodOrderItem { OrderItemId = 19, FoodOrderId = 19, MenuItemId = 19, Quantity = 4, UnitPrice = 535000.00m },
            new FoodOrderItem { OrderItemId = 20, FoodOrderId = 20, MenuItemId = 20, Quantity = 1, UnitPrice = 560000.00m }
        );

        modelBuilder.Entity<Kot>().HasData(
            new Kot { KotId = 1, FoodOrderId = 1, Status = KotStatus.Pending, CreatedAt = new DateTime(2026, 6, 3, 13, 20, 0) },
            new Kot { KotId = 2, FoodOrderId = 2, Status = KotStatus.Cooking, CreatedAt = new DateTime(2026, 6, 4, 14, 20, 0) },
            new Kot { KotId = 3, FoodOrderId = 3, Status = KotStatus.Ready, CreatedAt = new DateTime(2026, 6, 5, 15, 20, 0) },
            new Kot { KotId = 4, FoodOrderId = 4, Status = KotStatus.Completed, CreatedAt = new DateTime(2026, 6, 6, 16, 20, 0) },
            new Kot { KotId = 5, FoodOrderId = 5, Status = KotStatus.Cancelled, CreatedAt = new DateTime(2026, 6, 7, 17, 20, 0) },
            new Kot { KotId = 6, FoodOrderId = 6, Status = KotStatus.Pending, CreatedAt = new DateTime(2026, 6, 8, 18, 20, 0) },
            new Kot { KotId = 7, FoodOrderId = 7, Status = KotStatus.Cooking, CreatedAt = new DateTime(2026, 6, 9, 19, 20, 0) },
            new Kot { KotId = 8, FoodOrderId = 8, Status = KotStatus.Ready, CreatedAt = new DateTime(2026, 6, 10, 12, 20, 0) },
            new Kot { KotId = 9, FoodOrderId = 9, Status = KotStatus.Completed, CreatedAt = new DateTime(2026, 6, 11, 13, 20, 0) },
            new Kot { KotId = 10, FoodOrderId = 10, Status = KotStatus.Cancelled, CreatedAt = new DateTime(2026, 6, 12, 14, 20, 0) },
            new Kot { KotId = 11, FoodOrderId = 11, Status = KotStatus.Pending, CreatedAt = new DateTime(2026, 6, 13, 15, 20, 0) },
            new Kot { KotId = 12, FoodOrderId = 12, Status = KotStatus.Cooking, CreatedAt = new DateTime(2026, 6, 14, 16, 20, 0) },
            new Kot { KotId = 13, FoodOrderId = 13, Status = KotStatus.Ready, CreatedAt = new DateTime(2026, 6, 15, 17, 20, 0) },
            new Kot { KotId = 14, FoodOrderId = 14, Status = KotStatus.Completed, CreatedAt = new DateTime(2026, 6, 16, 18, 20, 0) },
            new Kot { KotId = 15, FoodOrderId = 15, Status = KotStatus.Cancelled, CreatedAt = new DateTime(2026, 6, 17, 19, 20, 0) },
            new Kot { KotId = 16, FoodOrderId = 16, Status = KotStatus.Pending, CreatedAt = new DateTime(2026, 6, 18, 12, 20, 0) },
            new Kot { KotId = 17, FoodOrderId = 17, Status = KotStatus.Cooking, CreatedAt = new DateTime(2026, 6, 19, 13, 20, 0) },
            new Kot { KotId = 18, FoodOrderId = 18, Status = KotStatus.Ready, CreatedAt = new DateTime(2026, 6, 20, 14, 20, 0) },
            new Kot { KotId = 19, FoodOrderId = 19, Status = KotStatus.Completed, CreatedAt = new DateTime(2026, 6, 21, 15, 20, 0) },
            new Kot { KotId = 20, FoodOrderId = 20, Status = KotStatus.Cancelled, CreatedAt = new DateTime(2026, 6, 22, 16, 20, 0) }
        );

        modelBuilder.Entity<Tour>().HasData(
            new Tour { TourId = 1, TourName = @"Island Hopping", Description = @"Island Hopping experience for resort guests.", Price = 425000.00m, MaxCapacity = 10, DurationHours = 3, Location = @"Phu Quoc Area 1", ImageUrl = @"/images/tours/tour-01.jpg" },
            new Tour { TourId = 2, TourName = @"Snorkeling Coral Reef", Description = @"Snorkeling Coral Reef experience for resort guests.", Price = 500000.00m, MaxCapacity = 12, DurationHours = 4, Location = @"Phu Quoc Area 2", ImageUrl = @"/images/tours/tour-02.jpg" },
            new Tour { TourId = 3, TourName = @"Sunset Fishing", Description = @"Sunset Fishing experience for resort guests.", Price = 575000.00m, MaxCapacity = 14, DurationHours = 5, Location = @"Phu Quoc Area 3", ImageUrl = @"/images/tours/tour-03.jpg" },
            new Tour { TourId = 4, TourName = @"Pearl Farm Visit", Description = @"Pearl Farm Visit experience for resort guests.", Price = 650000.00m, MaxCapacity = 16, DurationHours = 6, Location = @"Phu Quoc Area 4", ImageUrl = @"/images/tours/tour-04.jpg" },
            new Tour { TourId = 5, TourName = @"Forest Trekking", Description = @"Forest Trekking experience for resort guests.", Price = 725000.00m, MaxCapacity = 18, DurationHours = 7, Location = @"Phu Quoc Area 5", ImageUrl = @"/images/tours/tour-05.jpg" },
            new Tour { TourId = 6, TourName = @"Night Market Food Tour", Description = @"Night Market Food Tour experience for resort guests.", Price = 800000.00m, MaxCapacity = 20, DurationHours = 8, Location = @"Phu Quoc Area 6", ImageUrl = @"/images/tours/tour-06.jpg" },
            new Tour { TourId = 7, TourName = @"Kayak Lagoon", Description = @"Kayak Lagoon experience for resort guests.", Price = 875000.00m, MaxCapacity = 22, DurationHours = 2, Location = @"Phu Quoc Area 7", ImageUrl = @"/images/tours/tour-07.jpg" },
            new Tour { TourId = 8, TourName = @"Luxury Yacht Trip", Description = @"Luxury Yacht Trip experience for resort guests.", Price = 950000.00m, MaxCapacity = 8, DurationHours = 3, Location = @"Phu Quoc Area 8", ImageUrl = @"/images/tours/tour-08.jpg" },
            new Tour { TourId = 9, TourName = @"Historical Prison Tour", Description = @"Historical Prison Tour experience for resort guests.", Price = 1025000.00m, MaxCapacity = 10, DurationHours = 4, Location = @"Phu Quoc Area 9", ImageUrl = @"/images/tours/tour-09.jpg" },
            new Tour { TourId = 10, TourName = @"Pepper Farm Tour", Description = @"Pepper Farm Tour experience for resort guests.", Price = 1100000.00m, MaxCapacity = 12, DurationHours = 5, Location = @"Phu Quoc Area 10", ImageUrl = @"/images/tours/tour-10.jpg" },
            new Tour { TourId = 11, TourName = @"Cable Car Adventure", Description = @"Cable Car Adventure experience for resort guests.", Price = 1175000.00m, MaxCapacity = 14, DurationHours = 6, Location = @"Phu Quoc Area 11", ImageUrl = @"/images/tours/tour-11.jpg" },
            new Tour { TourId = 12, TourName = @"Diving Beginner", Description = @"Diving Beginner experience for resort guests.", Price = 1250000.00m, MaxCapacity = 16, DurationHours = 7, Location = @"Phu Quoc Area 12", ImageUrl = @"/images/tours/tour-12.jpg" },
            new Tour { TourId = 13, TourName = @"Diving Advanced", Description = @"Diving Advanced experience for resort guests.", Price = 1325000.00m, MaxCapacity = 18, DurationHours = 8, Location = @"Phu Quoc Area 13", ImageUrl = @"/images/tours/tour-13.jpg" },
            new Tour { TourId = 14, TourName = @"Private Beach Picnic", Description = @"Private Beach Picnic experience for resort guests.", Price = 1400000.00m, MaxCapacity = 20, DurationHours = 2, Location = @"Phu Quoc Area 14", ImageUrl = @"/images/tours/tour-14.jpg" },
            new Tour { TourId = 15, TourName = @"Photography Tour", Description = @"Photography Tour experience for resort guests.", Price = 1475000.00m, MaxCapacity = 22, DurationHours = 3, Location = @"Phu Quoc Area 15", ImageUrl = @"/images/tours/tour-15.jpg" },
            new Tour { TourId = 16, TourName = @"Wellness Retreat", Description = @"Wellness Retreat experience for resort guests.", Price = 1550000.00m, MaxCapacity = 8, DurationHours = 4, Location = @"Phu Quoc Area 16", ImageUrl = @"/images/tours/tour-16.jpg" },
            new Tour { TourId = 17, TourName = @"Cooking Class Tour", Description = @"Cooking Class Tour experience for resort guests.", Price = 1625000.00m, MaxCapacity = 10, DurationHours = 5, Location = @"Phu Quoc Area 17", ImageUrl = @"/images/tours/tour-17.jpg" },
            new Tour { TourId = 18, TourName = @"Cycling Countryside", Description = @"Cycling Countryside experience for resort guests.", Price = 1700000.00m, MaxCapacity = 12, DurationHours = 6, Location = @"Phu Quoc Area 18", ImageUrl = @"/images/tours/tour-18.jpg" },
            new Tour { TourId = 19, TourName = @"Bird Watching", Description = @"Bird Watching experience for resort guests.", Price = 1775000.00m, MaxCapacity = 14, DurationHours = 7, Location = @"Phu Quoc Area 19", ImageUrl = @"/images/tours/tour-19.jpg" },
            new Tour { TourId = 20, TourName = @"Family Discovery", Description = @"Family Discovery experience for resort guests.", Price = 1850000.00m, MaxCapacity = 16, DurationHours = 8, Location = @"Phu Quoc Area 20", ImageUrl = @"/images/tours/tour-20.jpg" }
        );

        modelBuilder.Entity<TourSchedule>().HasData(
            new TourSchedule { ScheduleId = 1, TourId = 1, TourDate = new DateTime(2026, 6, 11, 0, 0, 0), StartTime = new TimeSpan(8, 30, 0), AvailableSlots = 6, GuideId = 7 },
            new TourSchedule { ScheduleId = 2, TourId = 2, TourDate = new DateTime(2026, 6, 12, 0, 0, 0), StartTime = new TimeSpan(9, 0, 0), AvailableSlots = 7, GuideId = 7 },
            new TourSchedule { ScheduleId = 3, TourId = 3, TourDate = new DateTime(2026, 6, 13, 0, 0, 0), StartTime = new TimeSpan(10, 30, 0), AvailableSlots = 8, GuideId = 7 },
            new TourSchedule { ScheduleId = 4, TourId = 4, TourDate = new DateTime(2026, 6, 14, 0, 0, 0), StartTime = new TimeSpan(11, 0, 0), AvailableSlots = 9, GuideId = 7 },
            new TourSchedule { ScheduleId = 5, TourId = 5, TourDate = new DateTime(2026, 6, 15, 0, 0, 0), StartTime = new TimeSpan(12, 30, 0), AvailableSlots = 10, GuideId = 7 },
            new TourSchedule { ScheduleId = 6, TourId = 6, TourDate = new DateTime(2026, 6, 16, 0, 0, 0), StartTime = new TimeSpan(7, 0, 0), AvailableSlots = 11, GuideId = 7 },
            new TourSchedule { ScheduleId = 7, TourId = 7, TourDate = new DateTime(2026, 6, 17, 0, 0, 0), StartTime = new TimeSpan(8, 30, 0), AvailableSlots = 12, GuideId = 7 },
            new TourSchedule { ScheduleId = 8, TourId = 8, TourDate = new DateTime(2026, 6, 18, 0, 0, 0), StartTime = new TimeSpan(9, 0, 0), AvailableSlots = 13, GuideId = 7 },
            new TourSchedule { ScheduleId = 9, TourId = 9, TourDate = new DateTime(2026, 6, 19, 0, 0, 0), StartTime = new TimeSpan(10, 30, 0), AvailableSlots = 14, GuideId = 7 },
            new TourSchedule { ScheduleId = 10, TourId = 10, TourDate = new DateTime(2026, 6, 20, 0, 0, 0), StartTime = new TimeSpan(11, 0, 0), AvailableSlots = 5, GuideId = 7 },
            new TourSchedule { ScheduleId = 11, TourId = 11, TourDate = new DateTime(2026, 6, 21, 0, 0, 0), StartTime = new TimeSpan(12, 30, 0), AvailableSlots = 6, GuideId = 7 },
            new TourSchedule { ScheduleId = 12, TourId = 12, TourDate = new DateTime(2026, 6, 22, 0, 0, 0), StartTime = new TimeSpan(7, 0, 0), AvailableSlots = 7, GuideId = 7 },
            new TourSchedule { ScheduleId = 13, TourId = 13, TourDate = new DateTime(2026, 6, 23, 0, 0, 0), StartTime = new TimeSpan(8, 30, 0), AvailableSlots = 8, GuideId = 7 },
            new TourSchedule { ScheduleId = 14, TourId = 14, TourDate = new DateTime(2026, 6, 24, 0, 0, 0), StartTime = new TimeSpan(9, 0, 0), AvailableSlots = 9, GuideId = 7 },
            new TourSchedule { ScheduleId = 15, TourId = 15, TourDate = new DateTime(2026, 6, 25, 0, 0, 0), StartTime = new TimeSpan(10, 30, 0), AvailableSlots = 10, GuideId = 7 },
            new TourSchedule { ScheduleId = 16, TourId = 16, TourDate = new DateTime(2026, 6, 26, 0, 0, 0), StartTime = new TimeSpan(11, 0, 0), AvailableSlots = 11, GuideId = 7 },
            new TourSchedule { ScheduleId = 17, TourId = 17, TourDate = new DateTime(2026, 6, 27, 0, 0, 0), StartTime = new TimeSpan(12, 30, 0), AvailableSlots = 12, GuideId = 7 },
            new TourSchedule { ScheduleId = 18, TourId = 18, TourDate = new DateTime(2026, 6, 28, 0, 0, 0), StartTime = new TimeSpan(7, 0, 0), AvailableSlots = 13, GuideId = 7 },
            new TourSchedule { ScheduleId = 19, TourId = 19, TourDate = new DateTime(2026, 6, 29, 0, 0, 0), StartTime = new TimeSpan(8, 30, 0), AvailableSlots = 14, GuideId = 7 },
            new TourSchedule { ScheduleId = 20, TourId = 20, TourDate = new DateTime(2026, 6, 30, 0, 0, 0), StartTime = new TimeSpan(9, 0, 0), AvailableSlots = 5, GuideId = 7 }
        );

        modelBuilder.Entity<TourBooking>().HasData(
            new TourBooking { TourBookingId = 1, BookingId = 1, ScheduleId = 1, Participants = 2, TotalAmount = 850000.00m, BookingStatus = TourBookingStatus.Pending },
            new TourBooking { TourBookingId = 2, BookingId = 2, ScheduleId = 2, Participants = 3, TotalAmount = 1500000.00m, BookingStatus = TourBookingStatus.Confirmed },
            new TourBooking { TourBookingId = 3, BookingId = 3, ScheduleId = 3, Participants = 4, TotalAmount = 2300000.00m, BookingStatus = TourBookingStatus.Completed },
            new TourBooking { TourBookingId = 4, BookingId = 4, ScheduleId = 4, Participants = 5, TotalAmount = 3250000.00m, BookingStatus = TourBookingStatus.Cancelled },
            new TourBooking { TourBookingId = 5, BookingId = 5, ScheduleId = 5, Participants = 1, TotalAmount = 725000.00m, BookingStatus = TourBookingStatus.Pending },
            new TourBooking { TourBookingId = 6, BookingId = 6, ScheduleId = 6, Participants = 2, TotalAmount = 1600000.00m, BookingStatus = TourBookingStatus.Confirmed },
            new TourBooking { TourBookingId = 7, BookingId = 7, ScheduleId = 7, Participants = 3, TotalAmount = 2625000.00m, BookingStatus = TourBookingStatus.Completed },
            new TourBooking { TourBookingId = 8, BookingId = 8, ScheduleId = 8, Participants = 4, TotalAmount = 3800000.00m, BookingStatus = TourBookingStatus.Cancelled },
            new TourBooking { TourBookingId = 9, BookingId = 9, ScheduleId = 9, Participants = 5, TotalAmount = 5125000.00m, BookingStatus = TourBookingStatus.Pending },
            new TourBooking { TourBookingId = 10, BookingId = 10, ScheduleId = 10, Participants = 1, TotalAmount = 1100000.00m, BookingStatus = TourBookingStatus.Confirmed },
            new TourBooking { TourBookingId = 11, BookingId = 11, ScheduleId = 11, Participants = 2, TotalAmount = 2350000.00m, BookingStatus = TourBookingStatus.Completed },
            new TourBooking { TourBookingId = 12, BookingId = 12, ScheduleId = 12, Participants = 3, TotalAmount = 3750000.00m, BookingStatus = TourBookingStatus.Cancelled },
            new TourBooking { TourBookingId = 13, BookingId = 13, ScheduleId = 13, Participants = 4, TotalAmount = 5300000.00m, BookingStatus = TourBookingStatus.Pending },
            new TourBooking { TourBookingId = 14, BookingId = 14, ScheduleId = 14, Participants = 5, TotalAmount = 7000000.00m, BookingStatus = TourBookingStatus.Confirmed },
            new TourBooking { TourBookingId = 15, BookingId = 15, ScheduleId = 15, Participants = 1, TotalAmount = 1475000.00m, BookingStatus = TourBookingStatus.Completed },
            new TourBooking { TourBookingId = 16, BookingId = 16, ScheduleId = 16, Participants = 2, TotalAmount = 3100000.00m, BookingStatus = TourBookingStatus.Cancelled },
            new TourBooking { TourBookingId = 17, BookingId = 17, ScheduleId = 17, Participants = 3, TotalAmount = 4875000.00m, BookingStatus = TourBookingStatus.Pending },
            new TourBooking { TourBookingId = 18, BookingId = 18, ScheduleId = 18, Participants = 4, TotalAmount = 6800000.00m, BookingStatus = TourBookingStatus.Confirmed },
            new TourBooking { TourBookingId = 19, BookingId = 19, ScheduleId = 19, Participants = 5, TotalAmount = 8875000.00m, BookingStatus = TourBookingStatus.Completed },
            new TourBooking { TourBookingId = 20, BookingId = 20, ScheduleId = 20, Participants = 1, TotalAmount = 1850000.00m, BookingStatus = TourBookingStatus.Cancelled }
        );

        modelBuilder.Entity<Review>().HasData(
            new Review { ReviewId = 1, UserId = 1, TourId = 1, BookingId = null, Rating = 4, Comment = @"Sample review 01 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 16, 18, 0, 0) },
            new Review { ReviewId = 2, UserId = 2, TourId = null, BookingId = 2, Rating = 5, Comment = @"Sample review 02 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 17, 18, 0, 0) },
            new Review { ReviewId = 3, UserId = 3, TourId = 3, BookingId = null, Rating = 1, Comment = @"Sample review 03 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 18, 18, 0, 0) },
            new Review { ReviewId = 4, UserId = 4, TourId = null, BookingId = 4, Rating = 2, Comment = @"Sample review 04 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 19, 18, 0, 0) },
            new Review { ReviewId = 5, UserId = 5, TourId = 5, BookingId = null, Rating = 3, Comment = @"Sample review 05 for resort service scenario.", IsVisible = false, CreatedAt = new DateTime(2026, 6, 20, 18, 0, 0) },
            new Review { ReviewId = 6, UserId = 6, TourId = null, BookingId = 6, Rating = 4, Comment = @"Sample review 06 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 21, 18, 0, 0) },
            new Review { ReviewId = 7, UserId = 7, TourId = 7, BookingId = null, Rating = 5, Comment = @"Sample review 07 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 22, 18, 0, 0) },
            new Review { ReviewId = 8, UserId = 8, TourId = null, BookingId = 8, Rating = 1, Comment = @"Sample review 08 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 23, 18, 0, 0) },
            new Review { ReviewId = 9, UserId = 9, TourId = 9, BookingId = null, Rating = 2, Comment = @"Sample review 09 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 24, 18, 0, 0) },
            new Review { ReviewId = 10, UserId = 10, TourId = null, BookingId = 10, Rating = 3, Comment = @"Sample review 10 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 25, 18, 0, 0) },
            new Review { ReviewId = 11, UserId = 11, TourId = 11, BookingId = null, Rating = 4, Comment = @"Sample review 11 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 26, 18, 0, 0) },
            new Review { ReviewId = 12, UserId = 12, TourId = null, BookingId = 12, Rating = 5, Comment = @"Sample review 12 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 27, 18, 0, 0) },
            new Review { ReviewId = 13, UserId = 13, TourId = 13, BookingId = null, Rating = 1, Comment = @"Sample review 13 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 28, 18, 0, 0) },
            new Review { ReviewId = 14, UserId = 14, TourId = null, BookingId = 14, Rating = 2, Comment = @"Sample review 14 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 29, 18, 0, 0) },
            new Review { ReviewId = 15, UserId = 15, TourId = 15, BookingId = null, Rating = 3, Comment = @"Sample review 15 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 6, 30, 18, 0, 0) },
            new Review { ReviewId = 16, UserId = 16, TourId = null, BookingId = 16, Rating = 4, Comment = @"Sample review 16 for resort service scenario.", IsVisible = false, CreatedAt = new DateTime(2026, 7, 1, 18, 0, 0) },
            new Review { ReviewId = 17, UserId = 17, TourId = 17, BookingId = null, Rating = 5, Comment = @"Sample review 17 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 7, 2, 18, 0, 0) },
            new Review { ReviewId = 18, UserId = 18, TourId = null, BookingId = 18, Rating = 1, Comment = @"Sample review 18 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 7, 3, 18, 0, 0) },
            new Review { ReviewId = 19, UserId = 19, TourId = 19, BookingId = null, Rating = 2, Comment = @"Sample review 19 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 7, 4, 18, 0, 0) },
            new Review { ReviewId = 20, UserId = 20, TourId = null, BookingId = 20, Rating = 3, Comment = @"Sample review 20 for resort service scenario.", IsVisible = true, CreatedAt = new DateTime(2026, 7, 5, 18, 0, 0) }
        );

        modelBuilder.Entity<GuestFolio>().HasData(
            new GuestFolio { FolioId = 1, BookingId = 1, TotalRoomCharges = 2050000.00m, TotalFoodCharges = 170000.00m, TotalTourCharges = 850000.00m, TotalAmount = 3070000.00m, Status = FolioStatus.Unpaid },
            new GuestFolio { FolioId = 2, BookingId = 2, TotalRoomCharges = 2300000.00m, TotalFoodCharges = 330000.00m, TotalTourCharges = 1500000.00m, TotalAmount = 4130000.00m, Status = FolioStatus.PartialPaid },
            new GuestFolio { FolioId = 3, BookingId = 3, TotalRoomCharges = 2550000.00m, TotalFoodCharges = 540000.00m, TotalTourCharges = 2300000.00m, TotalAmount = 5390000.00m, Status = FolioStatus.Paid },
            new GuestFolio { FolioId = 4, BookingId = 4, TotalRoomCharges = 2800000.00m, TotalFoodCharges = 160000.00m, TotalTourCharges = 3250000.00m, TotalAmount = 6210000.00m, Status = FolioStatus.Refunded },
            new GuestFolio { FolioId = 5, BookingId = 5, TotalRoomCharges = 3050000.00m, TotalFoodCharges = 370000.00m, TotalTourCharges = 725000.00m, TotalAmount = 4145000.00m, Status = FolioStatus.Cancelled },
            new GuestFolio { FolioId = 6, BookingId = 6, TotalRoomCharges = 3300000.00m, TotalFoodCharges = 630000.00m, TotalTourCharges = 1600000.00m, TotalAmount = 5530000.00m, Status = FolioStatus.Unpaid },
            new GuestFolio { FolioId = 7, BookingId = 7, TotalRoomCharges = 3550000.00m, TotalFoodCharges = 940000.00m, TotalTourCharges = 2625000.00m, TotalAmount = 7115000.00m, Status = FolioStatus.PartialPaid },
            new GuestFolio { FolioId = 8, BookingId = 8, TotalRoomCharges = 3800000.00m, TotalFoodCharges = 260000.00m, TotalTourCharges = 3800000.00m, TotalAmount = 7860000.00m, Status = FolioStatus.Paid },
            new GuestFolio { FolioId = 9, BookingId = 9, TotalRoomCharges = 4050000.00m, TotalFoodCharges = 570000.00m, TotalTourCharges = 5125000.00m, TotalAmount = 9745000.00m, Status = FolioStatus.Refunded },
            new GuestFolio { FolioId = 10, BookingId = 10, TotalRoomCharges = 4300000.00m, TotalFoodCharges = 930000.00m, TotalTourCharges = 1100000.00m, TotalAmount = 6330000.00m, Status = FolioStatus.Cancelled },
            new GuestFolio { FolioId = 11, BookingId = 11, TotalRoomCharges = 4550000.00m, TotalFoodCharges = 1340000.00m, TotalTourCharges = 2350000.00m, TotalAmount = 8240000.00m, Status = FolioStatus.Unpaid },
            new GuestFolio { FolioId = 12, BookingId = 12, TotalRoomCharges = 4800000.00m, TotalFoodCharges = 360000.00m, TotalTourCharges = 3750000.00m, TotalAmount = 8910000.00m, Status = FolioStatus.PartialPaid },
            new GuestFolio { FolioId = 13, BookingId = 13, TotalRoomCharges = 5050000.00m, TotalFoodCharges = 770000.00m, TotalTourCharges = 5300000.00m, TotalAmount = 11120000.00m, Status = FolioStatus.Paid },
            new GuestFolio { FolioId = 14, BookingId = 14, TotalRoomCharges = 5300000.00m, TotalFoodCharges = 1230000.00m, TotalTourCharges = 7000000.00m, TotalAmount = 13530000.00m, Status = FolioStatus.Refunded },
            new GuestFolio { FolioId = 15, BookingId = 15, TotalRoomCharges = 5550000.00m, TotalFoodCharges = 1740000.00m, TotalTourCharges = 1475000.00m, TotalAmount = 8765000.00m, Status = FolioStatus.Cancelled },
            new GuestFolio { FolioId = 16, BookingId = 16, TotalRoomCharges = 5800000.00m, TotalFoodCharges = 460000.00m, TotalTourCharges = 3100000.00m, TotalAmount = 9360000.00m, Status = FolioStatus.Unpaid },
            new GuestFolio { FolioId = 17, BookingId = 17, TotalRoomCharges = 6050000.00m, TotalFoodCharges = 970000.00m, TotalTourCharges = 4875000.00m, TotalAmount = 11895000.00m, Status = FolioStatus.PartialPaid },
            new GuestFolio { FolioId = 18, BookingId = 18, TotalRoomCharges = 6300000.00m, TotalFoodCharges = 1530000.00m, TotalTourCharges = 6800000.00m, TotalAmount = 14630000.00m, Status = FolioStatus.Paid },
            new GuestFolio { FolioId = 19, BookingId = 19, TotalRoomCharges = 6550000.00m, TotalFoodCharges = 2140000.00m, TotalTourCharges = 8875000.00m, TotalAmount = 17565000.00m, Status = FolioStatus.Refunded },
            new GuestFolio { FolioId = 20, BookingId = 20, TotalRoomCharges = 6800000.00m, TotalFoodCharges = 560000.00m, TotalTourCharges = 1850000.00m, TotalAmount = 9210000.00m, Status = FolioStatus.Cancelled }
        );

        modelBuilder.Entity<FolioTransaction>().HasData(
            new FolioTransaction { TransactionId = 1, FolioId = 1, TransactionType = FolioTransactionType.RoomCharge, ReferenceId = 1, Amount = 350000.00m, CreatedAt = new DateTime(2026, 6, 17, 10, 30, 0) },
            new FolioTransaction { TransactionId = 2, FolioId = 2, TransactionType = FolioTransactionType.FoodCharge, ReferenceId = 2, Amount = 450000.00m, CreatedAt = new DateTime(2026, 6, 18, 10, 30, 0) },
            new FolioTransaction { TransactionId = 3, FolioId = 3, TransactionType = FolioTransactionType.TourCharge, ReferenceId = 3, Amount = 550000.00m, CreatedAt = new DateTime(2026, 6, 19, 10, 30, 0) },
            new FolioTransaction { TransactionId = 4, FolioId = 4, TransactionType = FolioTransactionType.Payment, ReferenceId = 4, Amount = 650000.00m, CreatedAt = new DateTime(2026, 6, 20, 10, 30, 0) },
            new FolioTransaction { TransactionId = 5, FolioId = 5, TransactionType = FolioTransactionType.Refund, ReferenceId = 5, Amount = 750000.00m, CreatedAt = new DateTime(2026, 6, 21, 10, 30, 0) },
            new FolioTransaction { TransactionId = 6, FolioId = 6, TransactionType = FolioTransactionType.Adjustment, ReferenceId = 6, Amount = 850000.00m, CreatedAt = new DateTime(2026, 6, 22, 10, 30, 0) },
            new FolioTransaction { TransactionId = 7, FolioId = 7, TransactionType = FolioTransactionType.RoomCharge, ReferenceId = 7, Amount = 950000.00m, CreatedAt = new DateTime(2026, 6, 23, 10, 30, 0) },
            new FolioTransaction { TransactionId = 8, FolioId = 8, TransactionType = FolioTransactionType.FoodCharge, ReferenceId = 8, Amount = 1050000.00m, CreatedAt = new DateTime(2026, 6, 24, 10, 30, 0) },
            new FolioTransaction { TransactionId = 9, FolioId = 9, TransactionType = FolioTransactionType.TourCharge, ReferenceId = 9, Amount = 1150000.00m, CreatedAt = new DateTime(2026, 6, 25, 10, 30, 0) },
            new FolioTransaction { TransactionId = 10, FolioId = 10, TransactionType = FolioTransactionType.Payment, ReferenceId = 10, Amount = 1250000.00m, CreatedAt = new DateTime(2026, 6, 26, 10, 30, 0) },
            new FolioTransaction { TransactionId = 11, FolioId = 11, TransactionType = FolioTransactionType.Refund, ReferenceId = 11, Amount = 1350000.00m, CreatedAt = new DateTime(2026, 6, 27, 10, 30, 0) },
            new FolioTransaction { TransactionId = 12, FolioId = 12, TransactionType = FolioTransactionType.Adjustment, ReferenceId = 12, Amount = 1450000.00m, CreatedAt = new DateTime(2026, 6, 28, 10, 30, 0) },
            new FolioTransaction { TransactionId = 13, FolioId = 13, TransactionType = FolioTransactionType.RoomCharge, ReferenceId = 13, Amount = 1550000.00m, CreatedAt = new DateTime(2026, 6, 29, 10, 30, 0) },
            new FolioTransaction { TransactionId = 14, FolioId = 14, TransactionType = FolioTransactionType.FoodCharge, ReferenceId = 14, Amount = 1650000.00m, CreatedAt = new DateTime(2026, 6, 30, 10, 30, 0) },
            new FolioTransaction { TransactionId = 15, FolioId = 15, TransactionType = FolioTransactionType.TourCharge, ReferenceId = 15, Amount = 1750000.00m, CreatedAt = new DateTime(2026, 7, 1, 10, 30, 0) },
            new FolioTransaction { TransactionId = 16, FolioId = 16, TransactionType = FolioTransactionType.Payment, ReferenceId = 16, Amount = 1850000.00m, CreatedAt = new DateTime(2026, 7, 2, 10, 30, 0) },
            new FolioTransaction { TransactionId = 17, FolioId = 17, TransactionType = FolioTransactionType.Refund, ReferenceId = 17, Amount = 1950000.00m, CreatedAt = new DateTime(2026, 7, 3, 10, 30, 0) },
            new FolioTransaction { TransactionId = 18, FolioId = 18, TransactionType = FolioTransactionType.Adjustment, ReferenceId = 18, Amount = 2050000.00m, CreatedAt = new DateTime(2026, 7, 4, 10, 30, 0) },
            new FolioTransaction { TransactionId = 19, FolioId = 19, TransactionType = FolioTransactionType.RoomCharge, ReferenceId = 19, Amount = 2150000.00m, CreatedAt = new DateTime(2026, 7, 5, 10, 30, 0) },
            new FolioTransaction { TransactionId = 20, FolioId = 20, TransactionType = FolioTransactionType.FoodCharge, ReferenceId = 20, Amount = 2250000.00m, CreatedAt = new DateTime(2026, 7, 6, 10, 30, 0) }
        );

        modelBuilder.Entity<Invoice>().HasData(
            new Invoice { InvoiceId = 1, FolioId = 1, InvoiceNumber = @"INV-2026-0001", TotalAmount = 2850000.00m, TaxAmount = 228000.00m, FinalAmount = 3078000.00m, InvoiceStatus = InvoiceStatus.Draft, IssuedAt = new DateTime(2026, 6, 19, 9, 0, 0) },
            new Invoice { InvoiceId = 2, FolioId = 2, InvoiceNumber = @"INV-2026-0002", TotalAmount = 3200000.00m, TaxAmount = 256000.00m, FinalAmount = 3456000.00m, InvoiceStatus = InvoiceStatus.Issued, IssuedAt = new DateTime(2026, 6, 20, 9, 0, 0) },
            new Invoice { InvoiceId = 3, FolioId = 3, InvoiceNumber = @"INV-2026-0003", TotalAmount = 3550000.00m, TaxAmount = 284000.00m, FinalAmount = 3834000.00m, InvoiceStatus = InvoiceStatus.Paid, IssuedAt = new DateTime(2026, 6, 21, 9, 0, 0) },
            new Invoice { InvoiceId = 4, FolioId = 4, InvoiceNumber = @"INV-2026-0004", TotalAmount = 3900000.00m, TaxAmount = 312000.00m, FinalAmount = 4212000.00m, InvoiceStatus = InvoiceStatus.Cancelled, IssuedAt = new DateTime(2026, 6, 22, 9, 0, 0) },
            new Invoice { InvoiceId = 5, FolioId = 5, InvoiceNumber = @"INV-2026-0005", TotalAmount = 4250000.00m, TaxAmount = 340000.00m, FinalAmount = 4590000.00m, InvoiceStatus = InvoiceStatus.Draft, IssuedAt = new DateTime(2026, 6, 23, 9, 0, 0) },
            new Invoice { InvoiceId = 6, FolioId = 6, InvoiceNumber = @"INV-2026-0006", TotalAmount = 4600000.00m, TaxAmount = 368000.00m, FinalAmount = 4968000.00m, InvoiceStatus = InvoiceStatus.Issued, IssuedAt = new DateTime(2026, 6, 24, 9, 0, 0) },
            new Invoice { InvoiceId = 7, FolioId = 7, InvoiceNumber = @"INV-2026-0007", TotalAmount = 4950000.00m, TaxAmount = 396000.00m, FinalAmount = 5346000.00m, InvoiceStatus = InvoiceStatus.Paid, IssuedAt = new DateTime(2026, 6, 25, 9, 0, 0) },
            new Invoice { InvoiceId = 8, FolioId = 8, InvoiceNumber = @"INV-2026-0008", TotalAmount = 5300000.00m, TaxAmount = 424000.00m, FinalAmount = 5724000.00m, InvoiceStatus = InvoiceStatus.Cancelled, IssuedAt = new DateTime(2026, 6, 26, 9, 0, 0) },
            new Invoice { InvoiceId = 9, FolioId = 9, InvoiceNumber = @"INV-2026-0009", TotalAmount = 5650000.00m, TaxAmount = 452000.00m, FinalAmount = 6102000.00m, InvoiceStatus = InvoiceStatus.Draft, IssuedAt = new DateTime(2026, 6, 27, 9, 0, 0) },
            new Invoice { InvoiceId = 10, FolioId = 10, InvoiceNumber = @"INV-2026-0010", TotalAmount = 6000000.00m, TaxAmount = 480000.00m, FinalAmount = 6480000.00m, InvoiceStatus = InvoiceStatus.Issued, IssuedAt = new DateTime(2026, 6, 28, 9, 0, 0) },
            new Invoice { InvoiceId = 11, FolioId = 11, InvoiceNumber = @"INV-2026-0011", TotalAmount = 6350000.00m, TaxAmount = 508000.00m, FinalAmount = 6858000.00m, InvoiceStatus = InvoiceStatus.Paid, IssuedAt = new DateTime(2026, 6, 29, 9, 0, 0) },
            new Invoice { InvoiceId = 12, FolioId = 12, InvoiceNumber = @"INV-2026-0012", TotalAmount = 6700000.00m, TaxAmount = 536000.00m, FinalAmount = 7236000.00m, InvoiceStatus = InvoiceStatus.Cancelled, IssuedAt = new DateTime(2026, 6, 30, 9, 0, 0) },
            new Invoice { InvoiceId = 13, FolioId = 13, InvoiceNumber = @"INV-2026-0013", TotalAmount = 7050000.00m, TaxAmount = 564000.00m, FinalAmount = 7614000.00m, InvoiceStatus = InvoiceStatus.Draft, IssuedAt = new DateTime(2026, 7, 1, 9, 0, 0) },
            new Invoice { InvoiceId = 14, FolioId = 14, InvoiceNumber = @"INV-2026-0014", TotalAmount = 7400000.00m, TaxAmount = 592000.00m, FinalAmount = 7992000.00m, InvoiceStatus = InvoiceStatus.Issued, IssuedAt = new DateTime(2026, 7, 2, 9, 0, 0) },
            new Invoice { InvoiceId = 15, FolioId = 15, InvoiceNumber = @"INV-2026-0015", TotalAmount = 7750000.00m, TaxAmount = 620000.00m, FinalAmount = 8370000.00m, InvoiceStatus = InvoiceStatus.Paid, IssuedAt = new DateTime(2026, 7, 3, 9, 0, 0) },
            new Invoice { InvoiceId = 16, FolioId = 16, InvoiceNumber = @"INV-2026-0016", TotalAmount = 8100000.00m, TaxAmount = 648000.00m, FinalAmount = 8748000.00m, InvoiceStatus = InvoiceStatus.Cancelled, IssuedAt = new DateTime(2026, 7, 4, 9, 0, 0) },
            new Invoice { InvoiceId = 17, FolioId = 17, InvoiceNumber = @"INV-2026-0017", TotalAmount = 8450000.00m, TaxAmount = 676000.00m, FinalAmount = 9126000.00m, InvoiceStatus = InvoiceStatus.Draft, IssuedAt = new DateTime(2026, 7, 5, 9, 0, 0) },
            new Invoice { InvoiceId = 18, FolioId = 18, InvoiceNumber = @"INV-2026-0018", TotalAmount = 8800000.00m, TaxAmount = 704000.00m, FinalAmount = 9504000.00m, InvoiceStatus = InvoiceStatus.Issued, IssuedAt = new DateTime(2026, 7, 6, 9, 0, 0) },
            new Invoice { InvoiceId = 19, FolioId = 19, InvoiceNumber = @"INV-2026-0019", TotalAmount = 9150000.00m, TaxAmount = 732000.00m, FinalAmount = 9882000.00m, InvoiceStatus = InvoiceStatus.Paid, IssuedAt = new DateTime(2026, 7, 7, 9, 0, 0) },
            new Invoice { InvoiceId = 20, FolioId = 20, InvoiceNumber = @"INV-2026-0020", TotalAmount = 9500000.00m, TaxAmount = 760000.00m, FinalAmount = 10260000.00m, InvoiceStatus = InvoiceStatus.Cancelled, IssuedAt = new DateTime(2026, 7, 8, 9, 0, 0) }
        );

        modelBuilder.Entity<Payment>().HasData(
            new Payment { PaymentId = 1, InvoiceId = 1, PaymentMethod = PaymentMethod.Cash, Amount = 1150000.00m, PaymentStatus = PaymentStatus.Pending, TransactionCode = @"TXN202600001", PaidAt = null },
            new Payment { PaymentId = 2, InvoiceId = 2, PaymentMethod = PaymentMethod.CreditCard, Amount = 1300000.00m, PaymentStatus = PaymentStatus.Success, TransactionCode = @"TXN202600002", PaidAt = new DateTime(2026, 6, 21, 14, 45, 0) },
            new Payment { PaymentId = 3, InvoiceId = 3, PaymentMethod = PaymentMethod.DebitCard, Amount = 1450000.00m, PaymentStatus = PaymentStatus.Failed, TransactionCode = @"TXN202600003", PaidAt = new DateTime(2026, 6, 22, 14, 45, 0) },
            new Payment { PaymentId = 4, InvoiceId = 4, PaymentMethod = PaymentMethod.VNPay, Amount = 1600000.00m, PaymentStatus = PaymentStatus.Refunded, TransactionCode = @"TXN202600004", PaidAt = new DateTime(2026, 6, 23, 14, 45, 0) },
            new Payment { PaymentId = 5, InvoiceId = 5, PaymentMethod = PaymentMethod.MoMo, Amount = 1750000.00m, PaymentStatus = PaymentStatus.Pending, TransactionCode = @"TXN202600005", PaidAt = null },
            new Payment { PaymentId = 6, InvoiceId = 6, PaymentMethod = PaymentMethod.BankTransfer, Amount = 1900000.00m, PaymentStatus = PaymentStatus.Success, TransactionCode = @"TXN202600006", PaidAt = new DateTime(2026, 6, 25, 14, 45, 0) },
            new Payment { PaymentId = 7, InvoiceId = 7, PaymentMethod = PaymentMethod.Cash, Amount = 2050000.00m, PaymentStatus = PaymentStatus.Failed, TransactionCode = @"TXN202600007", PaidAt = new DateTime(2026, 6, 26, 14, 45, 0) },
            new Payment { PaymentId = 8, InvoiceId = 8, PaymentMethod = PaymentMethod.CreditCard, Amount = 2200000.00m, PaymentStatus = PaymentStatus.Refunded, TransactionCode = @"TXN202600008", PaidAt = new DateTime(2026, 6, 27, 14, 45, 0) },
            new Payment { PaymentId = 9, InvoiceId = 9, PaymentMethod = PaymentMethod.DebitCard, Amount = 2350000.00m, PaymentStatus = PaymentStatus.Pending, TransactionCode = @"TXN202600009", PaidAt = null },
            new Payment { PaymentId = 10, InvoiceId = 10, PaymentMethod = PaymentMethod.VNPay, Amount = 2500000.00m, PaymentStatus = PaymentStatus.Success, TransactionCode = @"TXN202600010", PaidAt = new DateTime(2026, 6, 29, 14, 45, 0) },
            new Payment { PaymentId = 11, InvoiceId = 11, PaymentMethod = PaymentMethod.MoMo, Amount = 2650000.00m, PaymentStatus = PaymentStatus.Failed, TransactionCode = @"TXN202600011", PaidAt = new DateTime(2026, 6, 30, 14, 45, 0) },
            new Payment { PaymentId = 12, InvoiceId = 12, PaymentMethod = PaymentMethod.BankTransfer, Amount = 2800000.00m, PaymentStatus = PaymentStatus.Refunded, TransactionCode = @"TXN202600012", PaidAt = new DateTime(2026, 7, 1, 14, 45, 0) },
            new Payment { PaymentId = 13, InvoiceId = 13, PaymentMethod = PaymentMethod.Cash, Amount = 2950000.00m, PaymentStatus = PaymentStatus.Pending, TransactionCode = @"TXN202600013", PaidAt = null },
            new Payment { PaymentId = 14, InvoiceId = 14, PaymentMethod = PaymentMethod.CreditCard, Amount = 3100000.00m, PaymentStatus = PaymentStatus.Success, TransactionCode = @"TXN202600014", PaidAt = new DateTime(2026, 7, 3, 14, 45, 0) },
            new Payment { PaymentId = 15, InvoiceId = 15, PaymentMethod = PaymentMethod.DebitCard, Amount = 3250000.00m, PaymentStatus = PaymentStatus.Failed, TransactionCode = @"TXN202600015", PaidAt = new DateTime(2026, 7, 4, 14, 45, 0) },
            new Payment { PaymentId = 16, InvoiceId = 16, PaymentMethod = PaymentMethod.VNPay, Amount = 3400000.00m, PaymentStatus = PaymentStatus.Refunded, TransactionCode = @"TXN202600016", PaidAt = new DateTime(2026, 7, 5, 14, 45, 0) },
            new Payment { PaymentId = 17, InvoiceId = 17, PaymentMethod = PaymentMethod.MoMo, Amount = 3550000.00m, PaymentStatus = PaymentStatus.Pending, TransactionCode = @"TXN202600017", PaidAt = null },
            new Payment { PaymentId = 18, InvoiceId = 18, PaymentMethod = PaymentMethod.BankTransfer, Amount = 3700000.00m, PaymentStatus = PaymentStatus.Success, TransactionCode = @"TXN202600018", PaidAt = new DateTime(2026, 7, 7, 14, 45, 0) },
            new Payment { PaymentId = 19, InvoiceId = 19, PaymentMethod = PaymentMethod.Cash, Amount = 3850000.00m, PaymentStatus = PaymentStatus.Failed, TransactionCode = @"TXN202600019", PaidAt = new DateTime(2026, 7, 8, 14, 45, 0) },
            new Payment { PaymentId = 20, InvoiceId = 20, PaymentMethod = PaymentMethod.CreditCard, Amount = 4000000.00m, PaymentStatus = PaymentStatus.Refunded, TransactionCode = @"TXN202600020", PaidAt = new DateTime(2026, 7, 9, 14, 45, 0) }
        );

        modelBuilder.Entity<Refund>().HasData(
            new Refund { RefundId = 1, BookingId = 1, RefundAmount = 125000.00m, RefundReason = @"Room issue compensation", RefundStatus = RefundStatus.Pending, CreatedAt = new DateTime(2026, 6, 21, 11, 0, 0) },
            new Refund { RefundId = 2, BookingId = 2, RefundAmount = 150000.00m, RefundReason = @"Tour cancelled by weather", RefundStatus = RefundStatus.Approved, CreatedAt = new DateTime(2026, 6, 22, 11, 0, 0) },
            new Refund { RefundId = 3, BookingId = 3, RefundAmount = 175000.00m, RefundReason = @"Payment duplicate", RefundStatus = RefundStatus.Rejected, CreatedAt = new DateTime(2026, 6, 23, 11, 0, 0) },
            new Refund { RefundId = 4, BookingId = 4, RefundAmount = 200000.00m, RefundReason = @"Guest cancelled early", RefundStatus = RefundStatus.Completed, CreatedAt = new DateTime(2026, 6, 24, 11, 0, 0) },
            new Refund { RefundId = 5, BookingId = 5, RefundAmount = 225000.00m, RefundReason = @"Room issue compensation", RefundStatus = RefundStatus.Pending, CreatedAt = new DateTime(2026, 6, 25, 11, 0, 0) },
            new Refund { RefundId = 6, BookingId = 6, RefundAmount = 250000.00m, RefundReason = @"Tour cancelled by weather", RefundStatus = RefundStatus.Approved, CreatedAt = new DateTime(2026, 6, 26, 11, 0, 0) },
            new Refund { RefundId = 7, BookingId = 7, RefundAmount = 275000.00m, RefundReason = @"Payment duplicate", RefundStatus = RefundStatus.Rejected, CreatedAt = new DateTime(2026, 6, 27, 11, 0, 0) },
            new Refund { RefundId = 8, BookingId = 8, RefundAmount = 300000.00m, RefundReason = @"Guest cancelled early", RefundStatus = RefundStatus.Completed, CreatedAt = new DateTime(2026, 6, 28, 11, 0, 0) },
            new Refund { RefundId = 9, BookingId = 9, RefundAmount = 325000.00m, RefundReason = @"Room issue compensation", RefundStatus = RefundStatus.Pending, CreatedAt = new DateTime(2026, 6, 29, 11, 0, 0) },
            new Refund { RefundId = 10, BookingId = 10, RefundAmount = 350000.00m, RefundReason = @"Tour cancelled by weather", RefundStatus = RefundStatus.Approved, CreatedAt = new DateTime(2026, 6, 30, 11, 0, 0) },
            new Refund { RefundId = 11, BookingId = 11, RefundAmount = 375000.00m, RefundReason = @"Payment duplicate", RefundStatus = RefundStatus.Rejected, CreatedAt = new DateTime(2026, 7, 1, 11, 0, 0) },
            new Refund { RefundId = 12, BookingId = 12, RefundAmount = 400000.00m, RefundReason = @"Guest cancelled early", RefundStatus = RefundStatus.Completed, CreatedAt = new DateTime(2026, 7, 2, 11, 0, 0) },
            new Refund { RefundId = 13, BookingId = 13, RefundAmount = 425000.00m, RefundReason = @"Room issue compensation", RefundStatus = RefundStatus.Pending, CreatedAt = new DateTime(2026, 7, 3, 11, 0, 0) },
            new Refund { RefundId = 14, BookingId = 14, RefundAmount = 450000.00m, RefundReason = @"Tour cancelled by weather", RefundStatus = RefundStatus.Approved, CreatedAt = new DateTime(2026, 7, 4, 11, 0, 0) },
            new Refund { RefundId = 15, BookingId = 15, RefundAmount = 475000.00m, RefundReason = @"Payment duplicate", RefundStatus = RefundStatus.Rejected, CreatedAt = new DateTime(2026, 7, 5, 11, 0, 0) },
            new Refund { RefundId = 16, BookingId = 16, RefundAmount = 500000.00m, RefundReason = @"Guest cancelled early", RefundStatus = RefundStatus.Completed, CreatedAt = new DateTime(2026, 7, 6, 11, 0, 0) },
            new Refund { RefundId = 17, BookingId = 17, RefundAmount = 525000.00m, RefundReason = @"Room issue compensation", RefundStatus = RefundStatus.Pending, CreatedAt = new DateTime(2026, 7, 7, 11, 0, 0) },
            new Refund { RefundId = 18, BookingId = 18, RefundAmount = 550000.00m, RefundReason = @"Tour cancelled by weather", RefundStatus = RefundStatus.Approved, CreatedAt = new DateTime(2026, 7, 8, 11, 0, 0) },
            new Refund { RefundId = 19, BookingId = 19, RefundAmount = 575000.00m, RefundReason = @"Payment duplicate", RefundStatus = RefundStatus.Rejected, CreatedAt = new DateTime(2026, 7, 9, 11, 0, 0) },
            new Refund { RefundId = 20, BookingId = 20, RefundAmount = 600000.00m, RefundReason = @"Guest cancelled early", RefundStatus = RefundStatus.Completed, CreatedAt = new DateTime(2026, 7, 10, 11, 0, 0) }
        );

        modelBuilder.Entity<Notification>().HasData(
            new Notification { NotificationId = 1, UserId = 1, Title = @"Notification 01", Message = @"Sample notification message 01 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 2, 9, 0, 0) },
            new Notification { NotificationId = 2, UserId = 2, Title = @"Notification 02", Message = @"Sample notification message 02 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 3, 10, 0, 0) },
            new Notification { NotificationId = 3, UserId = 3, Title = @"Notification 03", Message = @"Sample notification message 03 for booking, payment, or service update.", IsRead = true, SentAt = new DateTime(2026, 6, 4, 11, 0, 0) },
            new Notification { NotificationId = 4, UserId = 4, Title = @"Notification 04", Message = @"Sample notification message 04 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 5, 12, 0, 0) },
            new Notification { NotificationId = 5, UserId = 5, Title = @"Notification 05", Message = @"Sample notification message 05 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 6, 13, 0, 0) },
            new Notification { NotificationId = 6, UserId = 6, Title = @"Notification 06", Message = @"Sample notification message 06 for booking, payment, or service update.", IsRead = true, SentAt = new DateTime(2026, 6, 7, 14, 0, 0) },
            new Notification { NotificationId = 7, UserId = 7, Title = @"Notification 07", Message = @"Sample notification message 07 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 8, 15, 0, 0) },
            new Notification { NotificationId = 8, UserId = 8, Title = @"Notification 08", Message = @"Sample notification message 08 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 9, 16, 0, 0) },
            new Notification { NotificationId = 9, UserId = 9, Title = @"Notification 09", Message = @"Sample notification message 09 for booking, payment, or service update.", IsRead = true, SentAt = new DateTime(2026, 6, 10, 17, 0, 0) },
            new Notification { NotificationId = 10, UserId = 10, Title = @"Notification 10", Message = @"Sample notification message 10 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 11, 8, 0, 0) },
            new Notification { NotificationId = 11, UserId = 11, Title = @"Notification 11", Message = @"Sample notification message 11 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 12, 9, 0, 0) },
            new Notification { NotificationId = 12, UserId = 12, Title = @"Notification 12", Message = @"Sample notification message 12 for booking, payment, or service update.", IsRead = true, SentAt = new DateTime(2026, 6, 13, 10, 0, 0) },
            new Notification { NotificationId = 13, UserId = 13, Title = @"Notification 13", Message = @"Sample notification message 13 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 14, 11, 0, 0) },
            new Notification { NotificationId = 14, UserId = 14, Title = @"Notification 14", Message = @"Sample notification message 14 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 15, 12, 0, 0) },
            new Notification { NotificationId = 15, UserId = 15, Title = @"Notification 15", Message = @"Sample notification message 15 for booking, payment, or service update.", IsRead = true, SentAt = new DateTime(2026, 6, 16, 13, 0, 0) },
            new Notification { NotificationId = 16, UserId = 16, Title = @"Notification 16", Message = @"Sample notification message 16 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 17, 14, 0, 0) },
            new Notification { NotificationId = 17, UserId = 17, Title = @"Notification 17", Message = @"Sample notification message 17 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 18, 15, 0, 0) },
            new Notification { NotificationId = 18, UserId = 18, Title = @"Notification 18", Message = @"Sample notification message 18 for booking, payment, or service update.", IsRead = true, SentAt = new DateTime(2026, 6, 19, 16, 0, 0) },
            new Notification { NotificationId = 19, UserId = 19, Title = @"Notification 19", Message = @"Sample notification message 19 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 20, 17, 0, 0) },
            new Notification { NotificationId = 20, UserId = 20, Title = @"Notification 20", Message = @"Sample notification message 20 for booking, payment, or service update.", IsRead = false, SentAt = new DateTime(2026, 6, 21, 8, 0, 0) }
        );

    }
}
