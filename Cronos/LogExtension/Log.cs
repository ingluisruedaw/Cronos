using System.Diagnostics;
using System.Globalization;
using Cronos.Resources;

namespace Cronos.LogExtension;

/// <summary>
/// Class Log.
/// </summary>
public class Log
{
    #region Keys Config
    /// <summary>
    /// Path write log file.
    /// </summary>
    private readonly string Path;

    /// <summary>
    /// Key FullFileName.
    /// </summary>
    private readonly string FullFileName;

    /// <summary>
    /// Key EndLineLog.
    /// </summary>
    private readonly string EndLineLog;

    /// <summary>
    /// Key LayerLineLog.
    /// </summary>
    private readonly string LayerLineLog;

    /// <summary>
    /// Key ClassLineLog.
    /// </summary>
    private readonly string ClassLineLog;

    /// <summary>
    /// Key FuncLineLog.
    /// </summary>
    private readonly string FuncLineLog;

    /// <summary>
    /// Key ErrorLineLog.
    /// </summary>
    private readonly string ErrorLineLog;
    #endregion

    #region File Config Parameters
    /// <summary>
    /// Class Name.
    /// </summary>
    private string ClassName { get; set; }

    /// <summary>
    /// Detail Error.
    /// </summary>
    private Exception Exception { get; set; }

    /// <summary>
    /// Function Name.
    /// </summary>
    private string FunctionName { get; set; }

    /// <summary>
    /// Layer Name.
    /// </summary>
    private string LayerName { get; set; }

    /// <summary>
    /// Project Name.
    /// </summary>
    private string ProjectName { get; set; }
    #endregion

    #region temp number
    /// <summary>
    /// Try To Write.
    /// </summary>
    private int TryToWrite { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor of class <seealso cref="Log"/>.
    /// </summary>
    [DebuggerStepThrough]
    public Log()
    {
        this.Path = LogConfiguration.Path;
        this.FullFileName = LogConfiguration.FullFileName;
        this.LayerLineLog = LogConfiguration.LayerLineLog;
        this.ClassLineLog = LogConfiguration.ClassLineLog;
        this.FuncLineLog = LogConfiguration.FuncLineLog;
        this.ErrorLineLog = LogConfiguration.ErrorLineLog;
        this.EndLineLog = LogConfiguration.EndLineLog;
    }

    /// <summary>
    /// Constructor of class <seealso cref="Log"/>.
    /// </summary>
    /// <param name="className">Class Name.</param>
    /// <param name="layerName">Layer Name.</param>
    /// <param name="projectName">Project Name.</param>
    [DebuggerStepThrough]
    public Log(string className, string layerName, string projectName) : this()
    {
        this.ClassName = className;
        this.LayerName = layerName;
        this.ProjectName = projectName;
    }
    #endregion

    #region Public
    /// <summary>
    /// Method validate for create file
    /// </summary>
    /// <param name="exception">exception.</param>
    /// <param name="functionName">function Name.</param>
    /// <returns>true or false.</returns>
    [DebuggerStepThrough]
    public bool Create(Exception exception, string functionName)
    {
        try
        {
            this.Exception = exception;
            this.FunctionName = functionName;

            if (this.TryToWrite <= 2)
            {
                if (this.VerifyPath() && this.WriteFile())
                {
                    this.TryToWrite = 1;
                    return true;
                }

                this.TryToWrite++;
                this.Create(this.Exception, this.FunctionName);
            }
            else
            {
                this.TryToWrite = 1;
            }

            return false;
        }
        catch (Exception)
        {
            this.TryToWrite = 1;
            return false;
        }
    }
    #endregion

    #region Private
    /// <summary>
    /// Format times hour, minutes, second.
    /// </summary>
    /// <param name="time">time.</param>
    /// <returns>time format.</returns>
    [DebuggerStepThrough]
    private string GetTimes(int time)
    {
        return string.Format(LogConfiguration.FormatTime, Convert.ToInt32(time, CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// Validate Directory Log.
    /// </summary>
    /// <returns>true or false.</returns>
    [DebuggerStepThrough]
    private bool VerifyPath()
    {
        try
        {
            if (Directory.Exists(this.Path + this.ProjectName))
            {
                return true;
            }

            Directory.CreateDirectory(this.Path + this.ProjectName);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Write file in Path
    /// </summary>
    /// <returns>true or false.</returns>
    [DebuggerStepThrough]
    private bool WriteFile()
    {
        try
        {
            DateTime datetime = DateTime.Now;
            List<object> data = new List<object>
            {
                this.Path,
                this.ProjectName,
                this.ProjectName,
                Convert.ToString(datetime.Date.ToString(LogConfiguration.FormatDateYmD, CultureInfo.CurrentCulture)),
                GetTimes(datetime.Hour)+ LogConfiguration.Point + GetTimes(datetime.Minute) + LogConfiguration.Point + GetTimes(datetime.Second)
            };

            StreamWriter write = new(string.Format(this.FullFileName, data.ToArray()), true);
            write.WriteLine(datetime.ToLongDateString() + LogConfiguration.Hyphen + datetime.ToLongTimeString());
            write.WriteLine(this.LayerLineLog + this.LayerName);
            write.WriteLine(this.ClassLineLog + this.ClassName);
            write.WriteLine(this.FuncLineLog + this.FunctionName);
            write.WriteLine(this.ErrorLineLog + this.Exception);
            write.WriteLine(this.EndLineLog);
            write.Close();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion
}
