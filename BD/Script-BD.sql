USE [master]
GO

/****** Object:  Database [BaseHeinsohn]    Script Date: 22/07/2024 12:29:15 a. m. ******/
CREATE DATABASE [BaseHeinsohn]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BaseHeinsohn', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BaseHeinsohn.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BaseHeinsohn_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BaseHeinsohn_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BaseHeinsohn].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BaseHeinsohn] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET ARITHABORT OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BaseHeinsohn] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BaseHeinsohn] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET  DISABLE_BROKER 
GO

ALTER DATABASE [BaseHeinsohn] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BaseHeinsohn] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [BaseHeinsohn] SET  MULTI_USER 
GO

ALTER DATABASE [BaseHeinsohn] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BaseHeinsohn] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BaseHeinsohn] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BaseHeinsohn] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [BaseHeinsohn] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BaseHeinsohn] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [BaseHeinsohn] SET QUERY_STORE = OFF
GO

ALTER DATABASE [BaseHeinsohn] SET  READ_WRITE 
GO

USE [BaseHeinsohn]
GO
/****** Object:  Table [dbo].[estado]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[estado](
	[est_id] [bigint] IDENTITY(1,1) NOT NULL,
	[est_nombre] [nvarchar](50) NOT NULL,
	[est_estado] [bit] NULL,
 CONSTRAINT [PK_estado] PRIMARY KEY CLUSTERED 
(
	[est_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tarea]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tarea](
	[tar_id] [bigint] IDENTITY(1,1) NOT NULL,
	[tar_nombre] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_tarea] PRIMARY KEY CLUSTERED 
(
	[tar_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tarea_estado]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tarea_estado](
	[tes_id] [bigint] IDENTITY(1,1) NOT NULL,
	[tes_tar_id] [bigint] NOT NULL,
	[tes_est_id] [bigint] NOT NULL,
	[tes_fecha] [datetime] NOT NULL,
	[tes_actual] [bit] NULL,
 CONSTRAINT [PK_tarea_estado] PRIMARY KEY CLUSTERED 
(
	[tes_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tarea_estado]  WITH CHECK ADD  CONSTRAINT [FK_tarea_estado_estado] FOREIGN KEY([tes_est_id])
REFERENCES [dbo].[estado] ([est_id])
GO
ALTER TABLE [dbo].[tarea_estado] CHECK CONSTRAINT [FK_tarea_estado_estado]
GO
ALTER TABLE [dbo].[tarea_estado]  WITH CHECK ADD  CONSTRAINT [FK_tarea_estado_tarea] FOREIGN KEY([tes_tar_id])
REFERENCES [dbo].[tarea] ([tar_id])
GO
ALTER TABLE [dbo].[tarea_estado] CHECK CONSTRAINT [FK_tarea_estado_tarea]
GO
/****** Object:  StoredProcedure [dbo].[estado_add_edit]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para registrar o modificar los estados dentro del sistema
-- =============================================
CREATE PROCEDURE [dbo].[estado_add_edit] 
	-- Parámetros del procedimiento
	@est_id bigint,
	@est_nombre nvarchar(50),
	@est_estado bit = null
AS
BEGIN
	SET NOCOUNT ON;

    -- Se realiza validación de si existe o no un estado con el mismo id para actualizarlo o crearlo
	MERGE estado AS v
		USING (SELECT @est_id AS Id, @est_nombre AS Nombre, @est_estado AS Estado) AS t
		ON v.est_id = t.id
	WHEN MATCHED THEN 
		UPDATE SET v.est_nombre = t.Nombre, v.est_estado = t.Estado
	WHEN NOT MATCHED THEN
		INSERT (est_nombre, est_estado)
		VALUES (t.Nombre, '1');
END
GO
/****** Object:  StoredProcedure [dbo].[estado_delete]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para eliminar (forma lógica) un estado que se encuentre registrado en el sistema
-- =============================================
CREATE PROCEDURE [dbo].[estado_delete] --1 
	-- Parámetros del procedimiento
	@est_id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    IF @est_id >0 BEGIN
        IF NOT EXISTS(SELECT * FROM dbo.estado WHERE est_id = @est_id) BEGIN
			RAISERROR('No se encontró información del Estado. Por favor verifique con el administrador del sistema!',16,1)
		    RETURN 0
        END
    END

	UPDATE dbo.estado
	SET  est_estado = 0 
	WHERE est_id = @est_id

END
GO
/****** Object:  StoredProcedure [dbo].[estado_get]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para consultar los estados registrados en el sistema
-- =============================================
CREATE PROCEDURE [dbo].[estado_get] --1, '', '0'
	-- Parámetros del procedimiento
	@est_id BIGINT = NULL,
	@est_criterio NVARCHAR(50) = NULL,
	@est_estado BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

    -- Se crea una variable para armar la query de modo escalable variando los parametros
	DECLARE @Query NVARCHAR(max),@ponerWhere BIT = 1

	SET @Query = N'SELECT e.est_id, e.est_nombre, e.est_estado 
				FROM estado AS e ';

	IF (@est_id IS NOT NULL AND @est_id > 0)
	BEGIN
		SET @Query += 'WHERE e.est_id = @est_id '
		SET @ponerWhere = 0
	END

	IF(@est_criterio IS NOT NULL AND @est_criterio <> '')
	BEGIN
		SET @Query += IIF(@ponerWhere = 1, ' WHERE ', ' AND ') + '(e.est_id LIKE''%'' + @est_criterio + ''%'' OR e.est_nombre LIKE''%'' + @criterio + ''%'') '
		SET @ponerWhere = 0
	END

	IF (@est_estado IS NOT NULL)
	BEGIN
		SET @Query += IIF(@ponerWhere = 1, ' WHERE ', ' AND ') + 'e.est_estado = @est_estado '
		SET @ponerWhere = 0
	END

	SET @Query += 'ORDER BY e.est_id'
	
	EXEC SP_EXECUTESQL @Query,N'@est_id BIGINT, @est_criterio nvarchar(50), @est_estado BIT ', @est_id, @est_criterio, @est_estado

END
GO
/****** Object:  StoredProcedure [dbo].[tarea_add_edit]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para registrar o modificar las tareas dentro del sistema
-- =============================================
CREATE PROCEDURE [dbo].[tarea_add_edit]  --1, 'Tarea 1', 1
	-- Parámetros del procedimiento
	@tar_id BIGINT,
	@tar_nombre NVARCHAR(200),
	@tar_est_id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    -- Se realiza validación de si existe o no un estado con el mismo id para actualizarlo o crearlo
	IF EXISTS(SELECT * FROM dbo.tarea WHERE tar_id = @tar_id) BEGIN
		UPDATE tarea 
		SET tar_nombre = @tar_nombre
		WHERE tar_id = @tar_id

		-- Se declara variable y se obtiene el estado actual de la tarea
		DECLARE @est_id_actual BIGINT = 0

		SELECT TOP 1 @est_id_actual = tes_est_id
		FROM tarea_estado 
		WHERE tes_tar_id = @tar_id AND tes_actual = '1'

		-- Se valida si el estado está siendo modificado se inserta el nuevo estado con su fecha correspondiente
		IF (@tar_est_id <> @est_id_actual) BEGIN			
			UPDATE tarea_estado SET tes_actual = '0' WHERE tes_tar_id = @tar_id

			INSERT INTO tarea_estado(tes_tar_id, tes_est_id, tes_fecha, tes_actual)
			VALUES (@tar_id, @tar_est_id, GETDATE(), '1');
		END
    END
	ELSE BEGIN
		INSERT INTO tarea (tar_nombre)
		VALUES (@tar_nombre)

		SET @tar_id = SCOPE_IDENTITY();

		INSERT INTO tarea_estado(tes_tar_id, tes_est_id, tes_fecha, tes_actual)
		VALUES (@tar_id, @tar_est_id, GETDATE(), '1');
	END

END
GO
/****** Object:  StoredProcedure [dbo].[tarea_delete]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para eliminar una tarea que se encuentre registrado en el sistema
-- =============================================
CREATE PROCEDURE [dbo].[tarea_delete] --1 
	-- Parámetros del procedimiento
	@tar_id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    IF @tar_id >0 BEGIN
        IF NOT EXISTS(SELECT * FROM dbo.tarea WHERE tar_id = @tar_id) BEGIN
			RAISERROR('No se encontró información de la tarea. Por favor verifique con el administrador del sistema!',16,1)
		    RETURN 0
        END
    END

	DELETE dbo.tarea_estado WHERE tes_tar_id = @tar_id

	DELETE dbo.tarea WHERE tar_id = @tar_id
	
END
GO
/****** Object:  StoredProcedure [dbo].[tarea_estado_get]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para consultar las tareas registradas en el sistema
-- =============================================
CREATE PROCEDURE [dbo].[tarea_estado_get] --1
	-- Parámetros del procedimiento
	@tes_tar_id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    -- Se crea una variable para armar la query de modo escalable variando los parametros
	DECLARE @Query NVARCHAR(max),@ponerWhere BIT = 1

	SET @Query = N'SELECT te.tes_id, te.tes_tar_id, te.tes_est_id, t.tar_nombre, e.est_nombre, te.tes_fecha
				FROM tarea_estado AS te
				INNER JOIN tarea AS t ON t.tar_id = te.tes_tar_id
				INNER JOIN estado AS e ON e.est_id = te.tes_est_id ';

	IF (@tes_tar_id IS NOT NULL AND @tes_tar_id > 0)
	BEGIN
		SET @Query += 'WHERE te.tes_tar_id = @tes_tar_id '
		SET @ponerWhere = 0
	END 

	SET @Query += 'ORDER BY te.tes_fecha DESC'
	
	EXEC SP_EXECUTESQL @Query,N'@tes_tar_id BIGINT', @tes_tar_id

END
GO
/****** Object:  StoredProcedure [dbo].[tarea_get]    Script Date: 22/07/2024 12:27:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leonardo Castro
-- Create date: 20-07-2024
-- Description:	Procedimiento que se usa para consultar las tareas registradas en el sistema
-- =============================================
CREATE PROCEDURE [dbo].[tarea_get] --NULL, '' 
	-- Parámetros del procedimiento
	@tar_id BIGINT = NULL,
	@tar_criterio NVARCHAR(200) = NULL
AS
BEGIN
	SET NOCOUNT ON;

    -- Se crea una variable para armar la query de modo escalable variando los parametros
	DECLARE @Query NVARCHAR(max),@ponerWhere BIT = 1

	SET @Query = N'SELECT t.tar_id, t.tar_nombre, temp.tes_est_id, temp.est_nombre
					FROM tarea AS t
					INNER JOIN (SELECT te.tes_tar_id, te.tes_est_id, e.est_nombre
								FROM tarea_estado AS te
								INNER JOIN estado AS e ON e.est_id = te.tes_est_id
								WHERE te.tes_actual = ''1'') AS temp 
						ON temp.tes_tar_id = t.tar_id ';

	IF (@tar_id IS NOT NULL AND @tar_id > 0)
	BEGIN
		SET @Query += 'WHERE t.tar_id = @tar_id '
		SET @ponerWhere = 0
	END

	IF(@tar_criterio IS NOT NULL AND @tar_criterio <> '')
	BEGIN
		SET @Query += IIF(@ponerWhere = 1, ' WHERE ', ' AND ') + '(t.tar_id LIKE''%'' + @tar_criterio + ''%'' OR t.tar_nombre LIKE''%'' + @criterio + ''%'') '
		SET @ponerWhere = 0
	END

	SET @Query += 'ORDER BY t.tar_id'
	
	EXEC SP_EXECUTESQL @Query,N'@tar_id BIGINT, @tar_criterio nvarchar(200) ', @tar_id, @tar_criterio

END
GO
