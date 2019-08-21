# importing os module 
import os 
  
# Function to rename multiple files 
def main(): 
      
    for filename in os.listdir(".\\"): 
        dst = filename.replace("ç╞o", "ção")          
        print("From: " + filename + "to: " + dst)
        # rename() function will 
        # rename all the files 
        os.rename(filename, dst) 
  
# Driver Code 
if __name__ == '__main__': 
      
    # Calling main() function 
    main() 