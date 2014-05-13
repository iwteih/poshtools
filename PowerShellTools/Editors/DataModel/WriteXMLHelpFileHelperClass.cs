﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;


namespace HelpEditorOS
{
    public class WriteXMLHelpFileHelperClass
    {
        //private String XMLEncode(String encodedString)
        //{
        //    encodedString = encodedString.Replace("<", "&lt;");
        //    encodedString = encodedString.Replace("<", "&lt;");
        //    encodedString = encodedString.Replace("<", "&lt;");
        //    encodedString = encodedString.Replace("<", "&lt;");
        //    encodedString = encodedString.Replace("<", "&lt;");
        //    encodedString = encodedString.Replace("<", "&lt;");
        //}
        public XmlWriter writeCmdletDetails(XmlWriter writer, cmdletDescription result)
        {

            //ns.AddNamespace("", "http://msh");
            //ns.AddNamespace("command", "http://schemas.microsoft.com/maml/dev/command/2004/10");
            //ns.AddNamespace("maml", "http://schemas.microsoft.com/maml/2004/10");
            //ns.AddNamespace("dev","http://schemas.microsoft.com/maml/dev/2004/10");
            //String OutString = result.Members["Verb"].Value + "\n";
            //OutString += result.Members["Noun"].Value + "\n";
            //Write <commnd:details>
            //  foreach (cmdletDescription result in CmdletsHelps)
            //{
            try
            {
                writer.WriteRaw("<!--Generated by Help Cmdlet Editor-->\r\n");
                writer.WriteRaw("	<command:details>\r\n");
                writer.WriteRaw("		<command:name>");

                //Write cmdletName
                writer.WriteRaw(HttpUtility.HtmlEncode(result.CmdletName));
                writer.WriteRaw("</command:name>\r\n");

                //Short Description
                writer.WriteRaw("		<maml:description>\r\n");
                writer.WriteRaw("			<maml:para>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.ShortDescription));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("		</maml:description>\r\n");
                // writer.WriteRaw("\r\n    <maml:copyright>\r\n        <maml:para></maml:para>\r\n    </maml:copyright>\r\n");
                //Write CopyRight info:
                writer.WriteRaw("		<maml:copyright>\r\n");
                writer.WriteRaw("           <maml:para />\r\n");
                writer.WriteRaw("		<!--Add copy right info here.-->\r\n");
                //writer.WriteRaw();
                writer.WriteRaw("		</maml:copyright>\r\n");


                //Write Noun and Verb
                writer.WriteRaw("		<command:verb>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.Verb));
                writer.WriteRaw("</command:verb>\r\n");
                writer.WriteRaw("		<command:noun>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.Noun));
                writer.WriteRaw("</command:noun>\r\n");

                //Add Dev version
                writer.WriteRaw("		<!--Add Dev version info here.-->\r\n");
                writer.WriteRaw("		<dev:version />\r\n");
                //writer.WriteRaw();

                //End </commnd:details>
                writer.WriteRaw("	</command:details>\r\n");


                //Add Cmdlet detailed description
                writer.WriteRaw("	<maml:description>\r\n");
                writer.WriteRaw("	<!--This is the Description section-->\r\n");
                writer.WriteRaw("		<maml:para>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.LongDescription));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("	</maml:description>\r\n");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

            // writer.WriteRaw();

            // }
            return writer;

        }

        public XmlWriter createSyntaxItem(XmlWriter writer, parameterDecription param, cmdletDescription Cmdlet)
        {
            try
            {

                // foreach (PSObject result in results)
                // {
                // String LocalParamDescription = "";
                Boolean Globbing = param.Globbing;
                String cmdletName = Cmdlet.CmdletName; // TODO add real CmdletName CmdletsList.SelectedValue.ToString();
                //foreach (HelpEditor.parameterDecription param in parametersDescription)
                //{

                //    if ((param.Name.ToLower() == Parameterecord.Name.ToLower()) && (cmdletName == param.CmdletName))
                //    {
                //        LocalParamDescription = param.NewDescription;
                //        Globbing = param.Globbing;
                //        break;
                //    }
                //}





                // Parameter Metadata for the Syntax Item section.
                writer.WriteRaw("			<command:parameter ");
                writer.WriteRaw("required = \"" + param.isMandatory.ToString().ToLower() + "\"");
                //Do variable length verification
                String parametertype = param.ParameterType;
                int LengthName = parametertype.Length;
                String VariableLength = "false";

                if (LengthName > 2)
                {
                    if (parametertype[LengthName - 1] == ']' && parametertype[LengthName - 2] == '[')
                    {
                        VariableLength = "true";
                    }
                }

                writer.WriteRaw(" variableLength = \"" + VariableLength + "\"");
                // TODO add code for globbing here.
                String strGlobbing = "false";
                if (Globbing)
                {
                    strGlobbing = "true";
                }
                writer.WriteRaw(" globbing = \"" + strGlobbing + "\"");

                String pipelineInput;
                if (param.VFP || param.VFPBPN)
                {
                    pipelineInput = "true (";
                    if (param.VFP) { pipelineInput += "ByValue"; }
                    if (param.VFPBPN)
                    {
                        if (pipelineInput.Length > 6)
                        {
                            pipelineInput += ", ByPropertyName)";
                        }
                        else
                        {
                            pipelineInput += "ByPropertyName)";
                        }
                    }
                    else
                    {
                        pipelineInput += ")";
                    }
                }
                else
                {
                    pipelineInput = "false";
                }
                writer.WriteRaw(" pipelineInput = \"" + pipelineInput + "\"");

                //Positional?
                // int position;
                // if (param.Position < 0)
                // {
                //    writer.WriteRaw(" position = \"named\">");
                // }
                // else
                // {
                //     position = (Parameterecord.Position + 1);
                writer.WriteRaw(" position = \"" + param.Position + "\" >\r\n");
                // }


                //Maml parameter name.
                //  writer.WriteRaw("				<!--Parameter Name-->\r\n");
                writer.WriteRaw("				<maml:name>");

                writer.WriteRaw(HttpUtility.HtmlEncode(param.Name));
                writer.WriteRaw("</maml:name>\r\n");


                //Parameter description

                //  writer.WriteRaw("maml", "para", "http://schemas.microsoft.com/maml/2004/10");
                writer.WriteRaw("				<maml:description>\r\n");
                // writer.WriteRaw("				<!--Parameter Description->\r\n");
                //Get the description from the struc.
                writer.WriteRaw("					<maml:para>");

                writer.WriteRaw(HttpUtility.HtmlEncode(param.NewDescription));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("				</maml:description>\r\n");

                string paramValueRequired = "true";
                if (param.ParameterType.ToLower() != "switchparameter")
                {
                    //Additional parameter Values
                    if (param.ParameterType.ToLower() == "boolean")
                    {
                        paramValueRequired = "false";
                    }
                    //Additional parameter Values
                    writer.WriteRaw("			<command:parameterValue ");
                    writer.WriteRaw("required=\"" + paramValueRequired + "\"");
                    writer.WriteRaw(" variableLength = \"" + VariableLength + "\" >");
                    writer.WriteRaw(HttpUtility.HtmlEncode(param.ParameterType));
                    writer.WriteRaw("</command:parameterValue>\r\n");
                }

                //End <command:prameter>
                writer.WriteRaw("			</command:parameter>\r\n");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }


            //}
            return writer;

        }

        public XmlWriter createParameters(XmlWriter writer, parameterDecription parametersDescription)
        {
            try
            {
                if (!parametersDescription.HelpOnlyParameter)
                {
                    string parametertype = parametersDescription.ParameterType.ToString();
                    int LengthName = parametertype.Length;
                    String VariableLength = "false";
                    String strGlobbing = "false";
                    //String cmdletName = "CmdletName"; // TODO add real CmdletName CmdletsList.SelectedValue.ToString();
                    //  foreach (HelpEditor.parameterDecription param in parametersDescription)
                    //  {

                    //      if ((parametersDescription.Name.ToLower() == Parameterecord.Name.ToLower()) && (cmdletName == param.CmdletName))
                    //     {
                    //LocalParamDescription = param.NewDescription;
                    if (parametersDescription.Globbing) strGlobbing = "true";
                    //         break;
                    //     }
                    //  }

                    if (LengthName > 2)
                    {
                        if (parametertype[LengthName - 1] == ']' && parametertype[LengthName - 2] == '[')
                        {
                            VariableLength = "true";
                        }
                    }

                    String pipelineInput;
                    if (parametersDescription.VFP || parametersDescription.VFPBPN)
                    {
                        pipelineInput = "true (";
                        if (parametersDescription.VFP) { pipelineInput += "ByValue"; }
                        if (parametersDescription.VFPBPN)
                        {
                            if (pipelineInput.Length > 6)
                            {
                                pipelineInput += ", ByPropertyName)";
                            }
                            else
                            {
                                pipelineInput += "ByPropertyName)";
                            }
                        }
                        else
                        {
                            pipelineInput += ")";
                        }
                    }
                    else
                    {
                        pipelineInput = "false";
                    }
                    // Parameter Metadata for the Syntax Item section.
                    writer.WriteRaw("		<command:parameter ");
                    writer.WriteRaw("required=\"" + parametersDescription.isMandatory.ToString().ToLower() + "\"");
                    writer.WriteRaw(" variableLength=\"" + VariableLength + "\"");
                    writer.WriteRaw(" globbing=\"" + strGlobbing + "\"");
                    writer.WriteRaw(" pipelineInput=\"" + pipelineInput + "\"");
                    //int position;
                    //if (Parameterecord.Position < 0)
                    //{
                    //    writer.WriteRaw(" position=\"named\">\r\n");
                    //}
                    //else
                    //{
                    // position = (Parameterecord.Position + 1);
                    writer.WriteRaw(" position=\"" + parametersDescription.Position + "\">\r\n");
                    //  }

                    //Maml parameter name.
                    writer.WriteRaw("			<maml:name>");
                    //writer.WriteComment("Parameter Name");
                    writer.WriteRaw(HttpUtility.HtmlEncode(parametersDescription.Name));
                    writer.WriteRaw("</maml:name>\r\n");

                    //Parameter description

                    writer.WriteRaw("			<maml:description>\r\n");
                    //writer.WriteComment("Parameter Description");
                    writer.WriteRaw("				<maml:para>");
                    String LocalParamDescription = "";
                    //foreach (HelpEditor.parameterDecription param in parametersDescription)
                    //{
                    //    if (param.Name.ToLower() == Parameterecord.Name.ToLower()) // TODO add real CmdletName && (String) this.CmdletsList.SelectedValue == param.CmdletName)
                    //    {
                    LocalParamDescription = HttpUtility.HtmlEncode(parametersDescription.NewDescription);
                    //    }
                    //}
                    writer.WriteRaw(LocalParamDescription);
                    writer.WriteRaw("</maml:para>\r\n");
                    writer.WriteRaw("			</maml:description>\r\n");

                    string paramValueRequired = "true";
                    if (parametersDescription.ParameterType.ToLower() != "switchparameter")
                    {
                        if (parametersDescription.ParameterType.ToLower() == "boolean")
                        {
                            paramValueRequired = "false";
                        }
                        //Additional parameter Values
                        writer.WriteRaw("			<command:parameterValue ");
                        writer.WriteRaw("required=\"" + paramValueRequired + "\"");
                        writer.WriteRaw(" variableLength=\"" + VariableLength + "\">");
                        writer.WriteRaw(HttpUtility.HtmlEncode(parametersDescription.ParameterType));
                        writer.WriteRaw("</command:parameterValue>\r\n");
                    }

                    //Dev Type
                    writer.WriteRaw("			<dev:type>\r\n");
                    writer.WriteRaw("				<maml:name>");
                    //writer.WriteComment("Parameter Type");
                    writer.WriteRaw(HttpUtility.HtmlEncode(parametersDescription.ParameterType));
                    writer.WriteRaw("</maml:name>\r\n");//maml:name
                    writer.WriteRaw("				<maml:uri/>\r\n");
                    //writer.WriteComment("uri");
                    //writer.WriteRaw();//maml:uri
                    writer.WriteRaw("			</dev:type>\r\n");//dev:type

                    //Dev Default Value. //TODO
                    writer.WriteRaw("			<dev:defaultValue>");
                    //writer.WriteComment("Default Value");
                    writer.WriteRaw(HttpUtility.HtmlEncode(parametersDescription.DefaultValue));
                    writer.WriteRaw("</dev:defaultValue>\r\n");

                    //<command:parameter required="true" variableLength="false" globbing="true" pipelineInput="true (ByPropertyName)" position="1">
                    //  <maml:name><!-- Prameter Name --></maml:name>
                    //  <maml:description>
                    //    <maml:para><!-- Parameter Description --></maml:para>

                    //  </maml:description>
                    //  <command:parameterValue required="true" variableLength="false">string[]</command:parameterValue>
                    //  <dev:type>
                    //    <maml:name><!-- Prameter Type --></maml:name>
                    //    <maml:uri/>
                    //  </dev:type>
                    //  <dev:defaultValue><!-- Default Value --></dev:defaultValue>
                    //</command:parameter>


                    //End <command:parameter>
                    writer.WriteRaw("		</command:parameter>\r\n");
                }
                return writer;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return writer;
            }
        }


        public XmlWriter createInputSection(XmlWriter writer, cmdletDescription result)
        {

            try
            {
                //Input Types Section
                writer.WriteRaw("	<command:inputTypes>\r\n");
                writer.WriteRaw("		<command:inputType>\r\n");
                writer.WriteRaw("			<dev:type>\r\n");

                //Input Type
                writer.WriteRaw("				<maml:name>");
                //writer.WriteComment("Input Type");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.InputType));
                writer.WriteRaw("</maml:name>\r\n");

                //Input Uri
                writer.WriteRaw("				<maml:uri/>\r\n");
                // writer.WriteComment("Uri section not used");
                // writer.WriteRaw();

                //Input Type
                writer.WriteRaw("				<maml:description>\r\n");
                //writer.WriteComment("Input type description");
                writer.WriteRaw("					<maml:para>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.InputDesc));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("				</maml:description>\r\n");


                //End dev:type
                writer.WriteRaw("			</dev:type>\r\n");

                //End command:inputType
                writer.WriteRaw("			<maml:description></maml:description>\r\n");


                //End command:inputTypes section
                writer.WriteRaw("		</command:inputType>\r\n");
                writer.WriteRaw("	</command:inputTypes>\r\n");

                //<!-- Input - Output section-->
                //<command:inputTypes>
                //  <command:inputType>
                //    <dev:type>
                //      <maml:name><!--Input Type--></maml:name>
                //      <maml:uri/>
                //      <maml:description>
                //        <maml:para>
                //          <!-- description  -->
                //          <!-- Input Type Description -->
                //        </maml:para>
                //      </maml:description>
                //    </dev:type>
                //    <maml:description></maml:description>
                //  </command:inputType>
                //</command:inputTypes>
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

            return writer;
        }

        public XmlWriter createOutputSection(XmlWriter writer, cmdletDescription result)
        {
            try
            {
                //Input Types Section
                writer.WriteRaw("	<command:returnValues>\r\n");
                writer.WriteRaw("		<command:returnValue>\r\n");
                writer.WriteRaw("			<dev:type>\r\n");

                //Input Type
                writer.WriteRaw("				<maml:name>");
                //writer.WriteComment("Output Type");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.OutputType));
                writer.WriteRaw("</maml:name>\r\n");

                //Input Uri
                writer.WriteRaw("				<maml:uri />\r\n");
                //writer.WriteComment("Uri section not used");
                //writer.WriteRaw();

                //Input Type
                writer.WriteRaw("				<maml:description>\r\n");
                //writer.WriteComment("Output type description");
                writer.WriteRaw("					<maml:para>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.OutputDesc));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("				</maml:description>\r\n");


                //End dev:type
                writer.WriteRaw("			</dev:type>\r\n");

                //End command:inputType
                writer.WriteRaw("			<maml:description></maml:description>\r\n");

                writer.WriteRaw("		</command:returnValue>\r\n");


                //End command:inputTypes section
                writer.WriteRaw("	</command:returnValues>\r\n");

                //<command:returnValues>
                //  <command:returnValue>
                //    <dev:type>
                //      <maml:name><!-- Output Type --></maml:name>
                //      <maml:uri />
                //      <maml:description>
                //        <maml:para>
                //          <!-- Output type description  -->

                //        </maml:para>
                //      </maml:description>
                //    </dev:type>
                //    <maml:description></maml:description>
                //  </command:returnValue>
                //</command:returnValues>

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

            return writer;
        }

        public XmlWriter createAlertSetSection(XmlWriter writer, cmdletDescription result)
        {
            try
            {


                //Start AlertSet section
                writer.WriteRaw("	<maml:alertSet>\r\n");
                //writer.WriteComment("Notes Secion");
                writer.WriteRaw("		<maml:title></maml:title>\r\n");
                writer.WriteRaw("		<maml:alert>\r\n");
                writer.WriteRaw("			<maml:para>");
                //writer.WriteComment("Note Details");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.Note));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("		</maml:alert>\r\n");

                //End AlertSet section
                writer.WriteRaw("	</maml:alertSet>\r\n");

                //<maml:alertSet>
                //  <maml:title></maml:title>
                //  <maml:alert>
                //    <maml:para>
                //      <!-- Note details-->
                //    </maml:para>
                //  </maml:alert>
                //  <maml:alert>
                //    <maml:para></maml:para>
                //  </maml:alert>
                //</maml:alertSet>
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

            return writer;
        }

        public XmlWriter createExampleItemSection(XmlWriter writer, example result)
        {
            try
            {

                //Start a <command:example> section
                writer.WriteRaw("		<command:example>\r\n");
                //writer.WriteComment("Example item section");
                writer.WriteRaw("			<maml:title>\r\n");
                if (result.ExampleName != null)
                {
                    String ExampleTitle = HttpUtility.HtmlEncode("--------------  " + result.ExampleName.Replace("-", "").Trim() + " --------------");
                    writer.WriteRaw(ExampleTitle);
                }

                writer.WriteRaw("			</maml:title>\r\n");

                //Introduction
                writer.WriteRaw("			<maml:introduction>\r\n");
                writer.WriteRaw("				<maml:para>C:\\PS&gt;</maml:para>\r\n");
                writer.WriteRaw("C:\\PS&gt;");
                writer.WriteRaw("			</maml:introduction>\r\n");
                //  writer.WriteRaw();

                //Dev code section
                writer.WriteRaw("  			<dev:code>");
                //writer.WriteRaw("Command to run");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.ExampleCmd));
                writer.WriteRaw("</dev:code>\r\n");

                //Dev remarks: Example description
                writer.WriteRaw("  			<dev:remarks>\r\n");
                //writer.WriteComment("Example description");
                writer.WriteRaw("				<maml:para>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.ExampleDescription));
                writer.WriteRaw("</maml:para>\r\n");
                writer.WriteRaw("				<maml:para></maml:para>\r\n");
                writer.WriteRaw("				<maml:para></maml:para>\r\n");
                writer.WriteRaw("				<maml:para>");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.ExampleOutput));
                writer.WriteRaw("\r\n				</maml:para>\r\n");
                writer.WriteRaw("				<maml:para></maml:para>\r\n");
                writer.WriteRaw("  			</dev:remarks>\r\n");
                //writer.WriteRaw();

                //Example output section
                writer.WriteRaw("			<command:commandLines>\r\n");
                //writer.WriteComment("Example Output");
                writer.WriteRaw("				<command:commandLine>\r\n");
                writer.WriteRaw("					<command:commandText>\r\n");
                //writer.WriteComment("Example output section");
                //writer.WriteRaw(HttpUtility.HtmlEncode(result.ExampleOutput));
                writer.WriteRaw("					</command:commandText>\r\n");
                writer.WriteRaw("				</command:commandLine>\r\n");
                writer.WriteRaw("			</command:commandLines>\r\n");


                //End example secion
                writer.WriteRaw("		</command:example>\r\n");


                //<command:example>
                //  <maml:title>
                //    <!-- EXAMPLE 1-->
                //  </maml:title>
                //  <maml:introduction>
                //    <maml:para>C:\PS&gt;</maml:para>
                //  </maml:introduction>
                //  <dev:code><!-- Command to run --></dev:code>
                //  <dev:remarks>
                //    <maml:para><!-- Example description--></maml:para>
                //    <maml:para></maml:para>
                //    <maml:para></maml:para>
                //    <maml:para>      </maml:para>
                //    <maml:para></maml:para>
                //  </dev:remarks>
                //  <command:commandLines>
                //    <command:commandLine>
                //      <command:commandText><!-- Example output section--></command:commandText>
                //    </command:commandLine>
                //  </command:commandLines>
                //</command:example>
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

            return writer;
        }

        public XmlWriter createLinksSection(XmlWriter writer, relatedlink result)
        {
            try
            {

                writer.WriteRaw("		<maml:navigationLink>\r\n");

                writer.WriteRaw("			<maml:linkText>");
                //writer.WriteComment("Related link text");
                writer.WriteRaw(HttpUtility.HtmlEncode(result.LinkText));
                writer.WriteRaw("</maml:linkText>\r\n");


                //End related links section
                writer.WriteRaw("			<maml:uri/>\r\n");
                writer.WriteRaw("		</maml:navigationLink>\r\n");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error writing the XML File.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

            return writer;


        }
    }
}