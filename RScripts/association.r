#install.packages("arules")
library(arules)

#getwd()
print("Loading dataset...")
#df <- read.table("C:\\Users\\leosm\\Documents\\Projects\\TCC\\DataSet\\2018_dataset.csv", header= FALSE, sep = ";")
df <- read.table("C:\\Users\\leosm\\Documents\\Projects\\TCC\\mpe.csv", header= FALSE, sep = ";")
print("Dataset Succesfully loaded!")

print("Creating transactions...")
trans <- as(split(df[,"V2"], df[,'V1']), 'transactions')
print("Transactions created!")

#inspect(trans)

print("Running Apriori...")
df <- apriori(trans, parameter=list(minlen=3,supp=0.3,conf=0.6,target='rules'))
#df <- apriori(trans, parameter=list(minlen=2,supp=0.05,conf=0.3,target='rules'))
print("Apriori Success!")
#summary(df)
inspect(df)

#print("Saving rules...")
#write(df,
#      file = "C:\\Users\\leosm\\Documents\\Projects\\TCC\\DataSet\\results\\30-09-2019association_rules.csv",
#      sep = ";",
#      quote = TRUE,
#      row.names = FALSE)
#print("Finished!")


#install.packages('arulesViz')
#library('arulesViz')

#plot(df)

plot(df, method="graph", control=list(typle="items"))

#plot(df, method="grouped")