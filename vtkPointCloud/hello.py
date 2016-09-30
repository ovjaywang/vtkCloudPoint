import sys
print sys.path
sys.path.append(r'E:\\Program Files\\IronPython 2.7.4') 
sys.path.append(r'E:\\Program Files\\IronPython 2.7.4\\DLLs')
sys.path.append(r'E:\\Program Files\\IronPython 2.7.4\\Lib')
sys.path.append(r'E:\\Program Files\\IronPython 2.7.4\\Lib\\site-packages')
import clr
clr.AddReferenceToFileAndPath('E:\\Program Files\\IronPython 2.7.4\\DLLs\\NumpyDotNet.dll')
#clr.AddReferenceToFileAndPath('E:\\Program Files\\IronPython 2.7.4\\DLLs\\mtrand.dll')
import numpy
print numpy.__version__

def say_hello():
    print "hello!"

def get_text():
    return "text from hello.py"

def add(arg1, arg2):
    return arg1 + arg2

def getpath():
    sys.path.append(r"c:\python27\lib")
    print sys.path