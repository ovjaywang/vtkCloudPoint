import sys
def say_hello():
    print "hello!"

def get_text():
    return "text from hello.py"

def add(arg1, arg2):
    return arg1 + arg2

def getpath():
    sys.path.append(r"c:\python27\lib")
    print sys.path