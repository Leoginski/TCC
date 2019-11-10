#install.packages("arules")
#install.packages('arulesViz')
library(arules)
#library('arulesViz')

#getwd()
years <- c("2013","2014","2015","2016","2017","2018","2019")
months <- c("01","02","03","04","05","06","07","08","09","10","11","12")

mainFolder <- "C:\\Users\\leosm\\Documents\\Projects\\TCC"

for (year in years)
{
  for (month in months)
  {
    date <- stringr::str_interp("${year}\\${year}${month}")
    inputPath <- stringr::str_interp("${mainFolder}\\DataSetByMonth\\${date}.csv")
    
    df <- read.table(inputPath, header= FALSE, sep = ";")
    trans <- as(split(df[,"V2"], df[,'V1']), 'transactions')
    df <- apriori(trans, parameter=list(minlen=2, maxlen=10, supp=0.01,conf=0.3,target='rules'))
    
    #summary(df)
    #inspect(df)
    
    #plot(df)
    #plot(df, method="grouped")
    #plot(df, method="graph", control=list(typle="items"))
    
    outputPath <- stringr::str_interp("${mainFolder}\\RulesByMonth\\${date}_rules.csv")
    
    write(df,
          file = outputPath,
          sep = ";",
          quote = TRUE,
          row.names = FALSE)    
  }
}