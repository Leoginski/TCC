install.packages("arules")
library(arules)

df <- read.table("C:\\Users\\leosm\\Documents\\Projects\\TCC\\resultFiltered.csv", header= TRUE, sep = ";")

trans <- as(split(df[,"cnpj"], df[,'event']), 'transactions')
trans
inspect(trans)

AR <- apriori(trans, parameter=list(minlen=2,supp=0.03,conf=0.05,target='rules'))

summary(AR)
inspect(AR)

install.packages('arulesViz')
library('arulesViz')

plot(AR)

plot(AR, method="graph", control=list(typle="items"))

plot(AR, method="grouped")
