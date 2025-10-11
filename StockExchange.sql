USE [StockExchange]
GO
/****** Object:  Table [dbo].[BuyOrders]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuyOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[StockId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_BuyOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellOrders]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[StockId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockPriceHistory]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockPriceHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StockId] [int] NOT NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[RecordedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockPriceHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stocks]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stocks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[LastUpdated] [datetime] NULL,
	[TotalShares] [int] NOT NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[StockId] [int] NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[TotalAmount] [decimal](18, 3) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](100) NULL,
	[Balance] [decimal](18, 3) NOT NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserStock]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserStock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[StockId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[PurchasePrice] [decimal](18, 3) NOT NULL,
	[PurchasedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_UserStocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BuyOrders] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[BuyOrders] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellOrders] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SellOrders] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Stocks] ADD  CONSTRAINT [DF_Stocks_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Stocks] ADD  DEFAULT ((1000000)) FOR [TotalShares]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Balance]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[BuyOrders]  WITH CHECK ADD  CONSTRAINT [FK_BuyOrders_Stocks] FOREIGN KEY([StockId])
REFERENCES [dbo].[Stocks] ([Id])
GO
ALTER TABLE [dbo].[BuyOrders] CHECK CONSTRAINT [FK_BuyOrders_Stocks]
GO
ALTER TABLE [dbo].[BuyOrders]  WITH CHECK ADD  CONSTRAINT [FK_BuyOrders_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BuyOrders] CHECK CONSTRAINT [FK_BuyOrders_Users]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Stocks] FOREIGN KEY([StockId])
REFERENCES [dbo].[Stocks] ([Id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Stocks]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users]
GO
/****** Object:  StoredProcedure [dbo].[AddBuyOrder]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Alım Emri Ekle
CREATE PROCEDURE [dbo].[AddBuyOrder]
    @UserId INT,
    @StockId INT,
    @Quantity INT,
    @Price DECIMAL(18,3) -- Maksimum alım fiyatı
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- Bakiye kontrolü (tahmini)
        DECLARE @UserBalance DECIMAL(18,3)
        SELECT @UserBalance = Balance FROM Users WHERE Id = @UserId
        
        DECLARE @EstimatedCost DECIMAL(18,3) = @Quantity * @Price
        
        IF @UserBalance < @EstimatedCost
        BEGIN
            RAISERROR('Yetersiz bakiye', 16, 1)
            RETURN
        END
        
        -- Alım emrini ekle
        INSERT INTO BuyOrders (UserId, StockId, Quantity, Price)
        VALUES (@UserId, @StockId, @Quantity, @Price)
        
        -- Emir eşleştirmeyi tetikle
        EXEC MatchOrders @StockId
        
        COMMIT TRANSACTION
        
        SELECT 'TRUE' AS Success, 'Alım emri başarıyla eklendi' AS Message
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT 'FALSE' AS Success, ERROR_MESSAGE() AS Message
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[AddSellOrder]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddSellOrder]
    @UserId INT,
    @StockId INT,
    @Quantity INT,
    @Price DECIMAL(18,3) -- Minimum satış fiyatı
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- TOPLAM hisse miktarını kontrol et (tüm kayıtları topla)
        DECLARE @TotalAvailableQuantity INT
        SELECT @TotalAvailableQuantity = SUM(Quantity)
        FROM UserStock 
        WHERE UserId = @UserId AND StockId = @StockId
        
        IF @TotalAvailableQuantity IS NULL OR @TotalAvailableQuantity < @Quantity
        BEGIN
            RAISERROR('Yetersiz hisse miktarı. Mevcut: %d, Talep: %d', 16, 1, @TotalAvailableQuantity, @Quantity)
            RETURN
        END
        
        -- Satış emrini ekle
        INSERT INTO SellOrders (UserId, StockId, Quantity, Price)
        VALUES (@UserId, @StockId, @Quantity, @Price)
        
        -- Emir eşleştirmeyi tetikle
        EXEC MatchOrders @StockId
        
        COMMIT TRANSACTION
        
        SELECT 'TRUE' AS Success, 'Satış emri başarıyla eklendi' AS Message
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT 'FALSE' AS Success, ERROR_MESSAGE() AS Message
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[BuyStock]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Hisse Alım İşlemi
CREATE PROCEDURE [dbo].[BuyStock]
    @UserId INT,
    @StockId INT,
    @Quantity INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        
        DECLARE @CurrentPrice DECIMAL(18,3)
        DECLARE @TotalAmount DECIMAL(18,3)
        DECLARE @UserBalance DECIMAL(18,3)
        
        -- Mevcut fiyatı ve kullanıcı bakiyesini al
        SELECT @CurrentPrice = Price FROM Stocks WHERE Id = @StockId
        SELECT @UserBalance = Balance FROM Users WHERE Id = @UserId
        
        SET @TotalAmount = @Quantity * @CurrentPrice
        
        -- Bakiye kontrolü
        IF @UserBalance < @TotalAmount
        BEGIN
            RAISERROR('Yetersiz bakiye', 16, 1)
            RETURN
        END
        
        -- UserStock'u güncelle veya ekle
        IF EXISTS (SELECT 1 FROM UserStock WHERE UserId = @UserId AND StockId = @StockId)
        BEGIN
            -- Ortalama fiyatı güncelle
            UPDATE UserStock 
            SET 
                Quantity = Quantity + @Quantity,
                PurchasePrice = (
                    (PurchasePrice * Quantity) + (@CurrentPrice * @Quantity)
                ) / (Quantity + @Quantity)
            WHERE UserId = @UserId AND StockId = @StockId
        END
        ELSE
        BEGIN
            INSERT INTO UserStock (UserId, StockId, Quantity, PurchasePrice, PurchasedAt)
            VALUES (@UserId, @StockId, @Quantity, @CurrentPrice, GETDATE())
        END
        
        -- Transaction kaydı
        INSERT INTO Transactions (UserId, StockId, Type, Quantity, Price, TotalAmount)
        VALUES (@UserId, @StockId, 'BUY', @Quantity, @CurrentPrice, @TotalAmount)
        
        -- Bakiyeyi güncelle
        UPDATE Users SET Balance = Balance - @TotalAmount WHERE Id = @UserId
        
        COMMIT TRANSACTION
        
         SELECT 'SUCCESS' AS Result, @TotalAmount AS TotalAmount, @UserBalance-@TotalAmount AS NewBalance, @CurrentPrice AS ExecutedPrice
   
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS ErrorMessage
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[GetPortfolioSummary]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Portföy Özetini Getir
CREATE PROCEDURE [dbo].[GetPortfolioSummary]
    @UserId INT
AS
BEGIN
    DECLARE @TotalPortfolioValue DECIMAL(18,3)
    DECLARE @TotalInvestment DECIMAL(18,3)
    DECLARE @TotalProfitLoss DECIMAL(18,3)
    
    SELECT 
        @TotalPortfolioValue = SUM(us.Quantity * s.Price),
        @TotalInvestment = SUM(us.Quantity * us.PurchasePrice),
        @TotalProfitLoss = SUM((us.Quantity * s.Price) - (us.Quantity * us.PurchasePrice))
    FROM UserStock us
    INNER JOIN Stocks s ON us.StockId = s.Id
    WHERE us.UserId = @UserId AND us.Quantity > 0
    
    SELECT 
        ISNULL(@TotalPortfolioValue, 0) AS TotalPortfolioValue,
        ISNULL(@TotalInvestment, 0) AS TotalInvestment,
        ISNULL(@TotalProfitLoss, 0) AS TotalProfitLoss,
        CASE 
            WHEN @TotalInvestment > 0 THEN 
                (@TotalProfitLoss / @TotalInvestment) * 100
            ELSE 0 
        END AS TotalProfitLossPercent
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserPortfolio]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserPortfolio]
    @UserId INT
AS
BEGIN
    SELECT 
        s.Symbol,
        s.Name,
        s.Description,
        s.Price AS CurrentPrice,
        SUM(us.Quantity) AS Quantity,
        -- Ağırlıklı ortalama maliyet hesapla
        SUM(us.Quantity * us.PurchasePrice) / NULLIF(SUM(us.Quantity), 0) AS PurchasePrice,
        MIN(us.PurchasedAt) AS PurchasedAt,
        SUM(us.Quantity * s.Price) AS CurrentValue,
        SUM(us.Quantity * us.PurchasePrice) AS TotalCost,
        SUM(us.Quantity * s.Price) - SUM(us.Quantity * us.PurchasePrice) AS ProfitLoss,
        CASE 
            WHEN SUM(us.Quantity * us.PurchasePrice) > 0 THEN 
                ((SUM(us.Quantity * s.Price) - SUM(us.Quantity * us.PurchasePrice)) / SUM(us.Quantity * us.PurchasePrice)) * 100
            ELSE 0 
        END AS ProfitLossPercent
    FROM UserStock us
    INNER JOIN Stocks s ON us.StockId = s.Id
    WHERE us.UserId = @UserId AND us.Quantity > 0
    GROUP BY s.Symbol, s.Name, s.Description, s.Price
    ORDER BY CurrentValue DESC
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserStockPurchaseHistory]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserStockPurchaseHistory]
    @UserId INT,
    @StockSymbol NVARCHAR(50)
AS
BEGIN
    SELECT 
        us.Quantity,
        us.PurchasePrice,
        us.PurchasedAt,
        (us.Quantity * us.PurchasePrice) AS TotalCost,
        s.Price AS CurrentPrice,
        (us.Quantity * s.Price) AS CurrentValue,
        (us.Quantity * s.Price) - (us.Quantity * us.PurchasePrice) AS ProfitLoss
    FROM UserStock us
    INNER JOIN Stocks s ON us.StockId = s.Id
    WHERE us.UserId = @UserId AND us.Quantity > 0 AND s.Symbol = @StockSymbol
    ORDER BY us.PurchasedAt DESC
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserTransactions]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- İşlem Geçmişini Getir
CREATE PROCEDURE [dbo].[GetUserTransactions]
    @UserId INT
AS
BEGIN
    SELECT 
        t.Id,
        s.Symbol,
        s.Name AS StockName,
        t.Type,
        t.Quantity,
        t.Price,
        t.TotalAmount,
        t.TransactionDate
    FROM Transactions t
    INNER JOIN Stocks s ON t.StockId = s.Id
    WHERE t.UserId = @UserId
    ORDER BY t.TransactionDate DESC
END
GO
/****** Object:  StoredProcedure [dbo].[MatchOrders]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MatchOrders]
    @StockId INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- Eşleşebilecek emirleri bul
        DECLARE @MatchingOrders TABLE (
            BuyOrderId INT,
            BuyUserId INT,
            BuyQuantity INT,
            BuyPrice DECIMAL(18,3),
            SellOrderId INT,
            SellUserId INT,
            SellQuantity INT,
            SellPrice DECIMAL(18,3),
            MatchPrice DECIMAL(18,3)
        )
        
        INSERT INTO @MatchingOrders
        SELECT 
            bo.Id AS BuyOrderId,
            bo.UserId AS BuyUserId,
            bo.Quantity AS BuyQuantity,
            bo.Price AS BuyPrice,
            so.Id AS SellOrderId,
            so.UserId AS SellUserId,
            so.Quantity AS SellQuantity,
            so.Price AS SellPrice,
            so.Price AS MatchPrice
        FROM BuyOrders bo
        CROSS APPLY (
            SELECT TOP 1 *
            FROM SellOrders so
            WHERE so.StockId = @StockId 
              AND so.IsActive = 1
              AND so.Price <= bo.Price
            ORDER BY so.Price ASC, so.CreatedAt ASC
        ) so
        WHERE bo.StockId = @StockId 
          AND bo.IsActive = 1
          AND bo.Quantity > 0
        
        -- Eşleşen emirleri işle
        DECLARE @BuyOrderId INT, @BuyUserId INT, @BuyQuantity INT, @BuyPrice DECIMAL(18,3),
                @SellOrderId INT, @SellUserId INT, @SellQuantity INT, @SellPrice DECIMAL(18,3),
                @MatchPrice DECIMAL(18,3)
        
        DECLARE order_cursor CURSOR FOR
        SELECT BuyOrderId, BuyUserId, BuyQuantity, BuyPrice, 
               SellOrderId, SellUserId, SellQuantity, SellPrice, MatchPrice
        FROM @MatchingOrders
        ORDER BY MatchPrice ASC, SellOrderId ASC
        
        OPEN order_cursor
        FETCH NEXT FROM order_cursor INTO @BuyOrderId, @BuyUserId, @BuyQuantity, @BuyPrice,
                                         @SellOrderId, @SellUserId, @SellQuantity, @SellPrice, @MatchPrice
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @TransactionQuantity INT = CASE 
                WHEN @BuyQuantity <= @SellQuantity THEN @BuyQuantity 
                ELSE @SellQuantity 
            END
            
            DECLARE @TransactionAmount DECIMAL(18,3) = @TransactionQuantity * @MatchPrice
            
            -- 1. Emirleri güncelle
            UPDATE BuyOrders SET Quantity = Quantity - @TransactionQuantity WHERE Id = @BuyOrderId
            UPDATE SellOrders SET Quantity = Quantity - @TransactionQuantity WHERE Id = @SellOrderId
            
            -- 2. Satıcının UserStock'unu güncelle (FIFO mantığıyla)
            DECLARE @RemainingSellQuantity INT = @TransactionQuantity
            
            -- Satıcının hisselerini en eski alımdan başlayarak kullan
            DECLARE @UserStockId INT, @UserStockQuantity INT
            
            DECLARE stock_cursor CURSOR FOR
            SELECT Id, Quantity 
            FROM UserStock 
            WHERE UserId = @SellUserId AND StockId = @StockId AND Quantity > 0
            ORDER BY PurchasedAt ASC -- FIFO: İlk alınan ilk satılır
            
            OPEN stock_cursor
            FETCH NEXT FROM stock_cursor INTO @UserStockId, @UserStockQuantity
            
            WHILE @@FETCH_STATUS = 0 AND @RemainingSellQuantity > 0
            BEGIN
                DECLARE @UsedQuantity INT = CASE 
                    WHEN @UserStockQuantity <= @RemainingSellQuantity THEN @UserStockQuantity
                    ELSE @RemainingSellQuantity
                END
                
                -- UserStock'tan hisse düş
                UPDATE UserStock 
                SET Quantity = Quantity - @UsedQuantity
                WHERE Id = @UserStockId
                
                SET @RemainingSellQuantity = @RemainingSellQuantity - @UsedQuantity
                
                FETCH NEXT FROM stock_cursor INTO @UserStockId, @UserStockQuantity
            END
            
            CLOSE stock_cursor
            DEALLOCATE stock_cursor
            
            -- 3. Alıcının UserStock'unu güncelle veya ekle
            IF EXISTS (SELECT 1 FROM UserStock WHERE UserId = @BuyUserId AND StockId = @StockId)
            BEGIN
                -- Ortalama fiyatı güncelle
                UPDATE UserStock 
                SET 
                    Quantity = Quantity + @TransactionQuantity,
                    PurchasePrice = (
                        (PurchasePrice * Quantity) + (@MatchPrice * @TransactionQuantity)
                    ) / (Quantity + @TransactionQuantity)
                WHERE UserId = @BuyUserId AND StockId = @StockId
            END
            ELSE
            BEGIN
                INSERT INTO UserStock (UserId, StockId, Quantity, PurchasePrice, PurchasedAt)
                VALUES (@BuyUserId, @StockId, @TransactionQuantity, @MatchPrice, GETDATE())
            END
            
            -- 4. Transaction kayıtları
            INSERT INTO Transactions (UserId, StockId, Type, Quantity, Price, TotalAmount)
            VALUES (@BuyUserId, @StockId, 'BUY', @TransactionQuantity, @MatchPrice, @TransactionAmount)
            
            INSERT INTO Transactions (UserId, StockId, Type, Quantity, Price, TotalAmount)
            VALUES (@SellUserId, @StockId, 'SELL', @TransactionQuantity, @MatchPrice, @TransactionAmount)
            
            -- 5. Bakiyeleri güncelle
            UPDATE Users SET Balance = Balance - @TransactionAmount WHERE Id = @BuyUserId
            UPDATE Users SET Balance = Balance + @TransactionAmount WHERE Id = @SellUserId
            
            -- 6. Tamamen doldurulan emirleri pasif yap
            IF (SELECT Quantity FROM BuyOrders WHERE Id = @BuyOrderId) <= 0
                UPDATE BuyOrders SET IsActive = 0 WHERE Id = @BuyOrderId
                
            IF (SELECT Quantity FROM SellOrders WHERE Id = @SellOrderId) <= 0
                UPDATE SellOrders SET IsActive = 0 WHERE Id = @SellOrderId
            
            FETCH NEXT FROM order_cursor INTO @BuyOrderId, @BuyUserId, @BuyQuantity, @BuyPrice,
                                             @SellOrderId, @SellUserId, @SellQuantity, @SellPrice, @MatchPrice
        END
        
        CLOSE order_cursor
        DEALLOCATE order_cursor
        
        COMMIT TRANSACTION
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR(@ErrorMessage, 16, 1)
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SellStock]    Script Date: 11.10.2025 19:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Hisse Satış İşlemi
CREATE PROCEDURE [dbo].[SellStock]
    @UserId INT,
    @StockId INT,
    @Quantity INT,
    @CurrentPrice DECIMAL(18,3)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        
        DECLARE @AvailableQuantity INT
        DECLARE @PurchasePrice DECIMAL(18,3)
        DECLARE @TotalAmount DECIMAL(18,3)
        
        -- Mevcut hisse miktarını kontrol et
        SELECT @AvailableQuantity = Quantity, @PurchasePrice = PurchasePrice
        FROM UserStock 
        WHERE UserId = @UserId AND StockId = @StockId
        
        IF @AvailableQuantity IS NULL OR @AvailableQuantity < @Quantity
        BEGIN
            RAISERROR('Yetersiz hisse miktarı', 16, 1)
            RETURN
        END
        
        -- Satış tutarını hesapla
        SET @TotalAmount = @Quantity * @CurrentPrice
        
        -- UserStock'tan miktarı düş
        UPDATE UserStock 
        SET Quantity = Quantity - @Quantity
        WHERE UserId = @UserId AND StockId = @StockId
        
        -- Kullanıcı bakiyesini güncelle
        UPDATE Users 
        SET Balance = Balance + @TotalAmount
        WHERE Id = @UserId
        
        -- İşlem geçmişine ekle
        INSERT INTO Transactions (UserId, StockId, Type, Quantity, Price, TotalAmount)
        VALUES (@UserId, @StockId, 'SELL', @Quantity, @CurrentPrice, @TotalAmount)
        
        COMMIT TRANSACTION
        
        SELECT 'SUCCESS' AS Result, @TotalAmount AS Amount
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS ErrorMessage
    END CATCH
END
GO
