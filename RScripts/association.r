#install.packages("arules")
#install.packages('arulesViz')
library(arules)
library('arulesViz')

#getwd()
df <- read.table("C:\\Users\\leosm\\Documents\\Projects\\TCC\\DataSet\\2018_dataset.csv", header= FALSE, sep = ";")

#df <- read.table("C:\\Users\\leosm\\Documents\\Projects\\TCC\\mpeFull.csv", header= FALSE, sep = ";")
trans <- as(split(df[,"V2"], df[,'V1']), 'transactions')

#inspect(trans)

#df <- apriori(trans, parameter=list(minlen=2,supp=0.3,conf=0.4,target='rules'))
df <- apriori(trans, parameter=list(minlen=2,supp=0.03,conf=0.4,target='rules'))

summary(df)
inspect(df)

#plot(df)
#plot(df, method="grouped")
plot(df, method="graph", control=list(typle="items"))



#print("Saving rules...")
#write(df,
#      file = "C:\\Users\\leosm\\Documents\\Projects\\TCC\\DataSet\\results\\30-09-2019association_rules.csv",
#      sep = ";",
#      quote = TRUE,
#      row.names = FALSE)
#print("Finished!")