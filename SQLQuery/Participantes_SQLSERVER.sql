USE [transparencia]
GO

/****** Object:  Table [dbo].[ParticipanteLicitacao]    Script Date: 13/08/2019 09:26:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ParticipanteLicitacao](
	[Número Licitação] [varchar](max) NULL,
	[Número Processo] [varchar](max) NULL,
	[Código Órgão] [varchar](max) NULL,
	[Nome Órgão] [varchar](max) NULL,
	[Código UG] [varchar](max) NULL,
	[Nome UG] [varchar](max) NULL,
	[Código Item Compra] [varchar](max) NULL,
	[Descrição Item Compra] [varchar](max) NULL,
	[CNPJ Participante] [varchar](max) NULL,
	[Nome Participante] [varchar](max) NULL,
	[Flag Vencedor] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO