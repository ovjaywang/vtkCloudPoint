using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
namespace vtkPointCloud
{
    
class EnvUtil
{

    /// &lt;summary&gt;
    /// 设置系统环境变量
    /// &lt;/summary&gt;
    /// &lt;param name="name"&gt;变量名&lt;/param&gt;
    /// &lt;param name="strValue"&gt;值&lt;/param&gt;
    public static void SetSysEnvironment(string name, string strValue)
    {
        RegistryKey op = OpenSysEnvironment();
        op.SetValue(name, strValue);
        op.Close();

    }
    /// &lt;summary&gt;
    /// 检测系统环境变量是否存在
    /// &lt;/summary&gt;
    /// &lt;param name="name"&gt;&lt;/param&gt;
    /// &lt;returns&gt;&lt;/returns&gt;
    public bool CheckSysEnvironmentExist(string name)
    {
        if (!string.IsNullOrEmpty(GetSysEnvironmentByName(name)))
            return true;
        else
            return false;
    }

    /// &lt;summary&gt;
    /// 添加到PATH环境变量（会检测路径是否存在，存在就不重复）
    /// &lt;/summary&gt;
    /// &lt;param name="strPath"&gt;&lt;/param&gt;
    public static void SetPathAfter(string strHome)
    {
        string pathlist;
        pathlist = GetSysEnvironmentByName("PATH");
        //检测是否以;结尾
        if (pathlist.Substring(pathlist.Length - 1, 1) != ";")
        {
            SetSysEnvironment("PATH", pathlist + ";");
            pathlist = GetSysEnvironmentByName("PATH");
        }
        string[] list = pathlist.Split(';');
        bool isPathExist = false;

        foreach (string item in list)
        {
            if (item == strHome)
                isPathExist = true;
        }
        if (!isPathExist)
        {
            SetSysEnvironment("PATH", pathlist + strHome + ";");
        }

    }

    public static void SetPathBefore(string strHome)
    {

        string pathlist;
        pathlist = GetSysEnvironmentByName("PATH");
        string[] list = pathlist.Split(';');
        bool isPathExist = false;

        foreach (string item in list)
        {
            if (item == strHome)
                isPathExist = true;
        }
        if (!isPathExist)
        {
            SetSysEnvironment("PATH", strHome + ";" + pathlist);
        }

    }

    public static void SetPath(string strHome)
    {

        string pathlist;
        pathlist = GetSysEnvironmentByName("PATH");
        string[] list = pathlist.Split(';');
        bool isPathExist = false;

        foreach (string item in list)
        {
            if (item == strHome)
                isPathExist = true;
        }
        if (!isPathExist)
        {
            SetSysEnvironment("PATH", pathlist + strHome + ";");

        }

    }


    [DllImport("Kernel32.DLL ", SetLastError = true)]
    public static extern bool SetEnvironmentVariable(string lpName, string lpValue);

    public static string GetSysEnvironmentByName(string name)
    {
        string result = string.Empty;
        try
        {
            result = OpenSysEnvironment().GetValue(name).ToString();//读取
        }
        catch (Exception)
        {

            return string.Empty;
        }
        return result;

    }
    private static RegistryKey OpenSysEnvironment()
    {
        RegistryKey regLocalMachine = Registry.LocalMachine;
        RegistryKey regSYSTEM = regLocalMachine.OpenSubKey("SYSTEM", true);//打开HKEY_LOCAL_MACHINE下的SYSTEM 
        RegistryKey regControlSet001 = regSYSTEM.OpenSubKey("ControlSet001", true);//打开ControlSet001 
        RegistryKey regControl = regControlSet001.OpenSubKey("Control", true);//打开Control 
        RegistryKey regManager = regControl.OpenSubKey("Session Manager", true);//打开Control 

        RegistryKey regEnvironment = regManager.OpenSubKey("Environment", true);

        return regEnvironment;
    }

}
}
