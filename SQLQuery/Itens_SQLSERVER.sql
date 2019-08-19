USE [transparencia]
GO

/****** Object:  Table [dbo].[ItemLicitacao]    Script Date: 13/08/2019 09:26:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ItemLicitacao](
	[N�mero Licita��o] [varchar](max) NULL,
	[N�mero Processo] [varchar](max) NULL,
	[C�digo �rg�o] [varchar](max) NULL,
	[Nome �rg�o] [varchar](max) NULL,
	[C�digo UG] [varchar](max) NULL,
	[Nome UG] [varchar](max) NULL,
	[CNPJ Vencedor] [varchar](max) NULL,
	[Nome Vencedor] [varchar](max) NULL,
	[N�mero Item] [varchar](max) NULL,
	[Descri��o] [varchar](max) NULL,
	[Quantidade Item] [varchar](max) NULL,
	[Valor Item] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO