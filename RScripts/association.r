#install.packages("arules")
library(arules)

#getwd()
print("Loading dataset...")
df <- read.table("C:\\Users\\leosm\\Documents\\Projects\\TCC\\DataSet\\results\\30-09-2019resultFilteredToAssociation.csv", header= TRUE, sep = ";")
print("Dataset Succesfully loaded!")

print("Creating transactions...")
df <- as(split(df[,"cnpj"], df[,'event']), 'transactions')
print("Transactions created!")

#inspect(trans)

print("Running Apriori...")
df <- apriori(df, parameter=list(minlen=2,supp=0.03,conf=0.05,target='rules'))
print("Apriori Success!")

print("Saving rules...")
write(df,
      file = "C:\\Users\\leosm\\Documents\\Projects\\TCC\\DataSet\\results\\30-09-2019association_rules.csv",
      sep = ";",
      quote = TRUE,
      row.names = FALSE)
print("Finished!")

#summary(AR)
#inspect(AR)

#install.packages('arulesViz')
#library('arulesViz')

#plot(AR)

#plot(AR, method="graph", control=list(typle="items"))

#plot(AR, method="grouped")