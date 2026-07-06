USE PhucNguyenResortDB;
GO

DELETE FROM Reviews;
DBCC CHECKIDENT ('Reviews', RESEED, 0);
GO

DECLARE @tourId INT = 1;
DECLARE @i INT;

-- 1. Thêm 5 đánh giá cho mỗi Tour (1 -> 30)
WHILE @tourId <= 30
BEGIN
    SET @i = 1;
    WHILE @i <= 5
    BEGIN
        INSERT INTO Reviews (UserId, TourId, BookingId, Rating, Comment, IsVisible, CreatedAt, TourBookingId)
        VALUES (
            (ABS(CHECKSUM(NEWID())) % 20) + 1,
            @tourId, 
            NULL, 
            (ABS(CHECKSUM(NEWID())) % 2) + 4, -- 4 or 5
            N'Chuyến đi rất tuyệt vời, tổ chức chuyên nghiệp và hướng dẫn viên nhiệt tình!', 
            1, 
            GETDATE(), 
            NULL
        );
        SET @i = @i + 1;
    END
    SET @tourId = @tourId + 1;
END

-- 2. Thêm 5 đánh giá cho mỗi Phòng (thông qua BookingId của 20 phòng)
DECLARE @bookingId INT;
DECLARE @roomId INT;
DECLARE room_cursor CURSOR FOR 
SELECT DISTINCT RoomId FROM BookingDetails WHERE RoomId IS NOT NULL;

OPEN room_cursor;
FETCH NEXT FROM room_cursor INTO @roomId;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Lấy 1 BookingId bất kỳ cho phòng này
    SELECT TOP 1 @bookingId = BookingId FROM BookingDetails WHERE RoomId = @roomId;

    SET @i = 1;
    WHILE @i <= 5
    BEGIN
        INSERT INTO Reviews (UserId, TourId, BookingId, Rating, Comment, IsVisible, CreatedAt, TourBookingId)
        VALUES (
            (ABS(CHECKSUM(NEWID())) % 20) + 1, 
            NULL, 
            @bookingId, 
            (ABS(CHECKSUM(NEWID())) % 2) + 4, -- 4 or 5
            N'Phòng rất đẹp, sạch sẽ và đầy đủ tiện nghi, view xuất sắc!', 
            1, 
            GETDATE(), 
            NULL
        );
        SET @i = @i + 1;
    END

    FETCH NEXT FROM room_cursor INTO @roomId;
END

CLOSE room_cursor;
DEALLOCATE room_cursor;
GO
