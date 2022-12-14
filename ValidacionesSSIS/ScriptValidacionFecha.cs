#region Help:  Introduction to the script task
/* The Script Task allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services control flow. 
 * 
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script task. */
#endregion


#region Namespaces
using System;
using System.Data;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
#endregion

namespace ST_b9d8cf015355451097e3f23450fe9986
{
    /// <summary>
    /// ScriptMain is the entry point class of the script.  Do not change the name, attributes,
    /// or parent of this class.
    /// </summary>
	[Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
	public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
	{
        #region Help:  Using Integration Services variables and parameters in a script
        /* To use a variable in this script, first ensure that the variable has been added to 
         * either the list contained in the ReadOnlyVariables property or the list contained in 
         * the ReadWriteVariables property of this script task, according to whether or not your
         * code needs to write to the variable.  To add the variable, save this script, close this instance of
         * Visual Studio, and update the ReadOnlyVariables and 
         * ReadWriteVariables properties in the Script Transformation Editor window.
         * To use a parameter in this script, follow the same steps. Parameters are always read-only.
         * 
         * Example of reading from a variable:
         *  DateTime startTime = (DateTime) Dts.Variables["System::StartTime"].Value;
         * 
         * Example of writing to a variable:
         *  Dts.Variables["User::myStringVariable"].Value = "new value";
         * 
         * Example of reading from a package parameter:
         *  int batchId = (int) Dts.Variables["$Package::batchId"].Value;
         *  
         * Example of reading from a project parameter:
         *  int batchId = (int) Dts.Variables["$Project::batchId"].Value;
         * 
         * Example of reading from a sensitive project parameter:
         *  int batchId = (int) Dts.Variables["$Project::batchId"].GetSensitiveValue();
         * */

        #endregion

        #region Help:  Firing Integration Services events from a script
        /* This script task can fire events for logging purposes.
         * 
         * Example of firing an error event:
         *  Dts.Events.FireError(18, "Process Values", "Bad value", "", 0);
         * 
         * Example of firing an information event:
         *  Dts.Events.FireInformation(3, "Process Values", "Processing has started", "", 0, ref fireAgain)
         * 
         * Example of firing a warning event:
         *  Dts.Events.FireWarning(14, "Process Values", "No values received for input", "", 0);
         * */
        #endregion

        #region Help:  Using Integration Services connection managers in a script
        /* Some types of connection managers can be used in this script task.  See the topic 
         * "Working with Connection Managers Programatically" for details.
         * 
         * Example of using an ADO.Net connection manager:
         *  object rawConnection = Dts.Connections["Sales DB"].AcquireConnection(Dts.Transaction);
         *  SqlConnection myADONETConnection = (SqlConnection)rawConnection;
         *  //Use the connection in some code here, then release the connection
         *  Dts.Connections["Sales DB"].ReleaseConnection(rawConnection);
         *
         * Example of using a File connection manager
         *  object rawConnection = Dts.Connections["Prices.zip"].AcquireConnection(Dts.Transaction);
         *  string filePath = (string)rawConnection;
         *  //Use the connection in some code here, then release the connection
         *  Dts.Connections["Prices.zip"].ReleaseConnection(rawConnection);
         * */
        #endregion


        /// <summary>
        /// This method is called when this script task executes in the control flow.
        /// Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
        /// To open Help, press F1.
        /// </summary>
        public void Main()
        {
            // TODO: Add your code here

            StringBuilder lineas = new StringBuilder();
            Int32 v_error = 0;



            /* String fecha = Dts.Variables["User::pFechaProceso"].Value.ToString();
             String fechaProceso = Convert.ToDateTime(fecha.Substring(0, 4)  + fecha.Substring(4, 2) + fecha.Substring(6, 2)).ToString("yyyyddMM");
             String fechaProcesoF = Convert.ToDateTime(fecha.Substring(0, 4) + "-" + fecha.Substring(4, 2) + "-" + fecha.Substring(6, 2)).ToString("yyyy-MM-dd");//Variable para generar las salidas
             */
            String fechaProceso = Dts.Variables["User::pFechaProceso"].Value.ToString();
            try
            {
                DirectoryInfo di = new DirectoryInfo(Dts.Variables["User::RutaEntrad"].Value.ToString());

              
                //Validacion de fecha de archivos
                foreach (FileInfo file in di.GetFiles())
                {
                   
                    //MessageBox.Show("FechaComparativa: " + file.Name.Substring(25, 8));
                    //MessageBox.Show("FechaComparativa: " + file.Name.Substring(25, 8));
                    if (file.Name.StartsWith(Dts.Variables["User::PrefijoArchivo"].Value.ToString()) && file.Name.EndsWith(".zip"))
                    {
                      
                        if (file.Name.Substring(25,8) != fechaProceso)
                        {
                          
                            lineas.AppendLine("La fecha del archivo de entrada es incorrecta:"+ file.Name.ToString());

                            v_error = 1;
                        }
                    }
       
                }




            }
            catch (Exception E)
            {
                //MessageBox.Show(E.Message);
                v_error = 1;
            }

            if (v_error == 0)
            {
               // Dts.Variables["User::pFechaProcesoS"].Value = fechaProceso; //Variable para buscar archivos en el paso Renombrar
               //Dts.Variables["User::pFechaProcesoF"].Value = fechaProcesoF;


                Dts.TaskResult = (int)ScriptResults.Success;
               
            }
            else
            {
                Dts.Variables["User::pLogError"].Value = lineas;
            
                Dts.TaskResult = (int)ScriptResults.Failure;
                                
                
            }

        }

        #region ScriptResults declaration
        /// <summary>
        /// This enum provides a convenient shorthand within the scope of this class for setting the
        /// result of the script.
        /// 
        /// This code was generated automatically.
        /// </summary>
        enum ScriptResults
        {
            Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success,
            Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
        };
        #endregion

	}
}