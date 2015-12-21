using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace rebuildprojetos
{
    class temp
    {
        public XmlNode pai, chd;
    }
    class nodess
    {
        public string key;
        public XmlNode node;
    }
    class Program
    {
        static bool modificado;

        static void Main(string[] args)
        {
            var folderbase = Directory.GetCurrentDirectory();

            try
            {
                Console.WriteLine("---- START ----");
                Console.WriteLine(folderbase);
                foreach (var item in Directory.GetFiles(folderbase, "*.csproj", SearchOption.AllDirectories))
                {
                    if (item.Contains("NFe.") && !item.Contains("\\net35\\") && !item.Contains("\\net64\\"))
                    {
                        if (!item.EndsWith("3.5.csproj") && !item.EndsWith("64.csproj"))
                        {
                            changeProject(item, folderbase, "net35", "3.5.csproj");
                            changeProject(item, folderbase, "net64", "64.csproj");
                        }
                    }
                }
                Console.WriteLine("Done!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.ReadLine();
        }

        static void changeProject(string projectOriginal, string folderbase, string folderproject, string extensao)
        {
            string projectDestino;

            Console.Write(projectDestino = folderbase + "\\" + folderproject + projectOriginal.Replace(folderbase, "").Replace(".csproj", extensao));
            if (!File.Exists(projectDestino))
            {
                Console.WriteLine("  --> not found");
                return;
            }
            Console.WriteLine();

            string folderDestino = Path.GetDirectoryName(projectDestino);

            ///
            /// exclui as pastas que nao existem no projeto original
            /// 
            foreach (var res in Directory.GetDirectories(folderDestino, "*.", SearchOption.AllDirectories).
                        Where(p =>  !p.EndsWith("\\bin\\release", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\bin\\debug", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\obj\\release", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\obj\\debug", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\service references", StringComparison.InvariantCultureIgnoreCase) &&
                                    //!p.ToLower().Contains("\\obj\\") &&
                                    //!p.ToLower().Contains("\\bin\\") &&
                                    //!p.ToLower().Contains("\\properties") &&
                                    //!p.EndsWith("\\obj", StringComparison.InvariantCultureIgnoreCase) &&
                                    //!p.EndsWith("\\bin", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\properties", StringComparison.InvariantCultureIgnoreCase)))
            {
                string folderToCompare = Path.GetDirectoryName(projectOriginal) + res.Replace(folderDestino, "");
                if (!Directory.Exists(folderToCompare))
                {
                    Console.WriteLine("DELETE:  " + folderToCompare);
                    Directory.Delete(res, true);
                }
            }
            ///
            /// adiciona as pastas que existem no projeto original
            /// 
            foreach (var res in Directory.GetDirectories(Path.GetDirectoryName(projectOriginal), "*.", SearchOption.AllDirectories).
                        Where(p =>  !p.EndsWith("\\bin\\release", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\bin\\debug", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\obj\\release", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\obj\\debug", StringComparison.InvariantCultureIgnoreCase) &&
                                    //!p.ToLower().Contains("\\obj\\") &&
                                    //!p.ToLower().Contains("\\bin\\") &&
                                    //!p.ToLower().Contains("\\properties") && 
                                    !p.EndsWith("\\obj", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\bin", StringComparison.InvariantCultureIgnoreCase) &&
                                    !p.EndsWith("\\properties", StringComparison.InvariantCultureIgnoreCase)))
            {
                string folderToCreate = folderDestino + res.Replace(Path.GetDirectoryName(projectOriginal), "");
                if (!Directory.Exists(folderToCreate))
                {
                    Console.WriteLine("CREATE:  "+folderToCreate);
                    Directory.CreateDirectory(folderToCreate);
                }
            }


            string app_config1 = Path.GetDirectoryName(projectOriginal) + "\\app.config";
            if (File.Exists(app_config1))
                File.Copy(app_config1, folderDestino + "\\app.config", true);

            if (projectOriginal.EndsWith("NFe.Components.csproj"))
            {
                string settings_file1 = Path.GetDirectoryName(projectOriginal) + "\\Properties\\Settings.settings";
                if (File.Exists(settings_file1))
                    File.Copy(settings_file1, folderDestino + "\\Properties\\Settings.settings", true);

                settings_file1 = Path.GetDirectoryName(projectOriginal) + "\\Properties\\Settings.Designer.cs";
                if (File.Exists(settings_file1))
                    File.Copy(settings_file1, folderDestino + "\\Properties\\Settings.Designer.cs", true);

                ///
                /// exclui as pastas que nao sao mais usadas
                //foreach (var res in Directory.GetDirectories(folderDestino + "\\Web References"))
                //{
                //    if (!Directory.Exists(Path.GetDirectoryName(projectOriginal) + "\\Web References\\" + Path.GetFileName(res)))
                //    {
                //        Console.WriteLine("DELETE: " + res);
                //        Directory.Delete(res, true);
                //    }
                //}
                ///
                /// inclui/atualiza as pastas+arquivos
                foreach (var files in Directory.GetFiles(Path.GetDirectoryName(projectOriginal) + "\\Web References", "*.*", SearchOption.AllDirectories))
                {
                    string folder = Path.GetFileName(Path.GetDirectoryName(files));
                    string folder1 = folderDestino + "\\Web References\\" + folder;
                    //Console.WriteLine(folder1);
                    if (!Directory.Exists(folder1))
                    {
                        Console.WriteLine("CREATE: " + folder1);
                        Directory.CreateDirectory(folder1);
                    }
                    File.Copy(files, folder1 + "\\" + Path.GetFileName(files), true);
                }
            }

            if (File.Exists(Path.GetDirectoryName(projectOriginal) + "\\Properties\\Resources.Designer.cs"))
                File.Copy(Path.GetDirectoryName(projectOriginal) + "\\Properties\\Resources.Designer.cs", folderDestino + "\\Properties\\Resources.Designer.cs", true);

            if (File.Exists(Path.GetDirectoryName(projectOriginal) + "\\Properties\\Resources.resx"))
                File.Copy(Path.GetDirectoryName(projectOriginal) + "\\Properties\\Resources.resx", folderDestino + "\\Properties\\Resources.resx", true);

            if (projectOriginal.EndsWith("NFe.UI.csproj"))
            {
                ///
                /// copia os arquivos para o projeto de destino
                foreach (var res in Directory.GetFiles(Path.GetDirectoryName(projectOriginal) + "\\Resources"))
                {
                    File.Copy(res, folderDestino + "\\Resources\\" + Path.GetFileName(res), true);
                }
                ///
                /// exclui os arquivos nao usados
                foreach (var res in Directory.GetFiles(folderDestino + "\\Resources"))
                {
                    string todelete = Path.GetDirectoryName(projectOriginal) + "\\Resources\\" + Path.GetFileName(res);
                    if (!File.Exists(todelete))
                    {
                        File.Delete(res);
                    }
                }
            }

            string[] grupos = new string[] { "Compile", "EmbeddedResource", "None", "WebReferenceUrl", "Content" };
            List<temp> deletes = new List<temp>();
            Dictionary<string, XmlNode> aaa = new Dictionary<string, XmlNode>();

            string aprjname = Path.GetFileName(projectOriginal.Replace(folderbase, "").Replace(".csproj", ""));

            XmlDocument orig = new XmlDocument();
            orig.Load(projectOriginal);

            foreach (XmlElement i in orig.GetElementsByTagName("ItemGroup"))
            {
                foreach (var n_ode in grupos)
                {
                    foreach (XmlElement b in i.GetElementsByTagName(n_ode))
                    {
                        if (b.HasAttributes)
                        {
                            var attr = b.Attributes["Include"];
                            if (attr != null)
                            {
                                if (n_ode.Equals("None") && attr.Value.Equals("app.config"))
                                    continue;

                                try
                                {
                                    aaa.Add(n_ode + "|" + attr.Value, b);
                                }
                                catch (Exception ex) {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(n_ode + "|" + attr.Value+" -> "+ ex.Message);
                                    Console.ResetColor();
                                }
                            }
                        }
                    }
                }
            }
            //StringBuilder bb = new StringBuilder();
            //foreach (KeyValuePair<string, XmlNode> p in aaa)
            //{
            //    bb.AppendLine(p.Key+" | "+p.Value.ChildNodes.Count.ToString());
            //}
            //File.AppendAllText("e:\\temp\\aaaaaaa.txt", bb.ToString());

            XmlDocument dest = new XmlDocument();
            dest.Load(projectDestino);
            modificado = false;

            ///
            /// compara destino x original
            /// 
            foreach (XmlElement i in dest.GetElementsByTagName("ItemGroup"))
            {
                foreach (var n_ode in grupos)
                {
                    foreach (XmlElement b in i.GetElementsByTagName(n_ode))
                    {
                        if (b.HasAttributes)
                        {
                            var attr = b.Attributes["Include"];
                            if (attr == null) continue;

                            if (n_ode.Equals("None") && attr.Value.Equals("app.config"))
                                continue;

                            //if ((n_ode.Equals("None") || n_ode.Equals("WebReferenceUrl")) && !projectOriginal.EndsWith("NFe.Components.csproj"))
                            //    continue;

                            string atx = attr.Value;
                            if (attr.Value.StartsWith("..\\"))
                                atx = atx.Replace("..\\..\\", "").Replace(aprjname, "").Trim('\\');

                            try
                            {
                                var x = aaa[n_ode + "|" + atx];
                            }
                            catch
                            {
                                Console.WriteLine("EE: " + attr.Value);
                                deletes.Add(new temp() { pai = i, chd = b });
                                modificado = true;
                                //Console.WriteLine("EE: " + attr.Value);
                            }
                            continue;
#if false
                            if (attr.Value.StartsWith("Web References") || n_ode.Equals("WebReferenceUrl"))
                            {
                                try
                                {
                                    var x = aaa[n_ode + "|" + attr.Value];
                                }
                                catch
                                {
                                    Console.WriteLine("EE: " + attr.Value);
                                    deletes.Add(new temp() { pai = i, chd = b });
                                    modificado = true;
                                }
//                                bool achou = false;

//                                //Console.WriteLine(n_ode+"  "+attr.Value);

//                                var x_nodes = orig.GetElementsByTagName("ItemGroup");
//                                foreach (XmlElement ii in x_nodes)
//                                {
//                                    if (achou) break;
//                                    foreach (XmlElement bo in ii.GetElementsByTagName(n_ode))
//                                    {
//                                        if (bo.HasAttributes)
//                                        {
//                                            var attri = bo.Attributes["Include"];
//                                            if (attri != null)
//                                            {
//                                                if (attr.Value.Equals(attri.Value))
//                                                {
//                                                    achou = true;
//                                                    break;
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                                if (!achou)
//                                {
//                                    Console.WriteLine("NO_DELETE: " + n_ode + " -> " + attr.Value);
//                                    deletes.Add(new temp() { pai = i, chd = b });

//                                    ///
//                                    /// remove o nó nao usado
////                                    i.RemoveChild((XmlNode)b);
//                                    modificado = true;
//                                }
                            }
                            else
                            {
                                string prjname = Path.GetFileName(projectOriginal.Replace(folderbase, "").Replace(".csproj", ""));
                                string tx = attr.Value;
                                if (attr.Value.StartsWith("..\\"))
                                    tx = tx.Replace("..\\..\\", "").Replace(prjname, "").Trim('\\');

                                try
                                {
                                    var x = aaa[n_ode + "|" + tx];
                                }
                                catch
                                {
                                    Console.WriteLine("EE: " + attr.Value);
                                    deletes.Add(new temp() { pai = i, chd = b });
                                    modificado = true;
                                    //Console.WriteLine("EE: " + attr.Value);
                                }
                                if (attr.Value.StartsWith("..\\..\\") && b.HasChildNodes && b.FirstChild != null)
                                {

                                    //if (b.FirstChild.Name.Equals("Link"))
                                    //{
                                    //    if (!File.Exists(folderbase + attr.Value.Replace("..\\..\\", "\\")))
                                    //    {
                                    //        //Console.WriteLine(attr.Value);
                                    //        ///
                                    //        /// remove o nó nao usado
                                    //        //i.RemoveChild((XmlNode)b);
                                    //        deletes.Add(new temp() { pai = i, chd = b });
                                    //        modificado = true;
                                    //    }
                                    //}
                                }
                            }
#endif
                        }
                        //Console.WriteLine("----->" + attr);// ((XmlElement)b).OuterXml);
                    }
                }
            }

            ///
            /// exclui os nós nao usados no projeto original
            /// 
            foreach(temp xe in deletes)
                xe.pai.RemoveChild(xe.chd);

            ///
            /// compara o original x destino
            /// 
            foreach (KeyValuePair<string, XmlNode> p in aaa)
            {
                string n_ode = p.Key.Split('|')[0];
                string value = p.Key.Split('|')[1];
                bool found = false;
                foreach (XmlElement i in dest.GetElementsByTagName("ItemGroup"))
                {
                    foreach (XmlElement b in i.GetElementsByTagName(n_ode))
                    {
                        if (b.HasAttributes)
                        {
                            var attr = b.Attributes["Include"];
                            if (attr != null)
                            {
                                if (n_ode.Equals("None") && attr.Value.Equals("app.config"))
                                    continue;

                                if (value.Equals(attr.Value))
                                    found = true;
                                else
                                {
                                    var r = "..\\..\\" + aprjname + "\\" + value;
                                    if (r.Equals(attr.Value))
                                        found = true;
                                }
                            }
                        }
                        if (found) break;
                    }
                    if (found) break;
                }
                if (!found)// || (aprjname.Equals("NFe.UI") && (p.Key.StartsWith("None|Resources\\") || p.Key.StartsWith("Content|Resources\\"))))
                {
                    modificado = true;

                    var x_node = dest.CreateElement(n_ode, dest.DocumentElement.NamespaceURI);
                    XmlAttribute attrInclude = dest.CreateAttribute("Include");

                    //if (aprjname.Equals("NFe.UI") && (p.Key.StartsWith("None|Resources\\") || p.Key.StartsWith("Content|Resources\\")))
                    //{
                    //}
                    //else
                    if (n_ode == "WebReferenceUrl" || 
                        value.StartsWith("Web References") ||
                        p.Key.StartsWith("None|Resources\\") ||
                        p.Key.Contains("|Properties") ||
                        p.Key.EndsWith("\\Resources.resx")) 
                    {
                        if (p.Key.Contains("|Properties\\AssemblyInfo.cs"))
                        {
                            /// AssemblyInfo.cs é para usar o do projeto original
                        }
                        else
                        {
                            //<WebReferenceUrl Include="C:\Users\Renan\Desktop\NFSE\Presidente Prudente SP\PPresidentePrudenteSP-SIMPLISS.wsdl">
                            //  <UrlBehavior>Dynamic</UrlBehavior>
                            //  <RelPath>Web References\br.gov.sp.presidenteprudente.sistemas.www.p\</RelPath>
                            //  <UpdateFromURL>C:\Users\Renan\Desktop\NFSE\Presidente Prudente SP\PPresidentePrudenteSP-SIMPLISS.wsdl</UpdateFromURL>
                            //  <ServiceLocationURL>
                            //  </ServiceLocationURL>
                            //  <CachedDynamicPropName>
                            //  </CachedDynamicPropName>
                            //  <CachedAppSettingsObjectName>Settings</CachedAppSettingsObjectName>
                            //  <CachedSettingsPropName>NFe_Components_br_gov_sp_presidenteprudente_sistemas_www_p_NfseService</CachedSettingsPropName>
                            //</WebReferenceUrl>

                            attrInclude.Value = value;
                            x_node.Attributes.Append(attrInclude);

                            foreach (var item in p.Value.ChildNodes)
                            {
                                //Console.WriteLine(((XmlElement)item).Name + "   " + ((XmlElement)item).InnerText);

                                var z_node = dest.CreateElement(((XmlElement)item).Name, dest.DocumentElement.NamespaceURI);
                                z_node.InnerText = ((XmlElement)item).InnerText;
                                x_node.AppendChild(z_node);
                            }

                            var xx = get(dest, n_ode);
                            if (xx != null)
                                xx.AppendChild(x_node);
                            else
                            {
                                xx = get(dest, null);//ultimo ItemGroup
                                if (xx == null)
                                {
                                    var y_node = dest.CreateElement("ItemGroup", dest.DocumentElement.NamespaceURI);
                                    y_node.AppendChild(x_node);
                                    dest.DocumentElement.AppendChild(y_node);
                                }
                                else
                                    xx.AppendChild(x_node);
                            }
                            continue;
                        }
                    }
                    //<Compile Include="..\..\NFe.UI\Formularios\FormDummy.cs">
                    //  <Link>Formularios\FormDummy.cs</Link>
                    //  <SubType>Form</SubType>
                    //</Compile>
                    //<Compile Include="..\..\NFe.UI\Formularios\FormDummy.Designer.cs">
                    //  <Link>Formularios\FormDummy.Designer.cs</Link>
                    //  <DependentUpon>FormDummy.cs</DependentUpon>
                    //</Compile>

                    //<EmbeddedResource Include="..\..\NFe.UI\Formularios\NFSe\userGrid2.resx">
                    //  <Link>Formularios\NFSe\userGrid2.resx</Link>
                    //  <DependentUpon>userGrid2.cs</DependentUpon>
                    //</EmbeddedResource>

                    //bool depOn = false;
                    attrInclude.Value = @"..\..\" + aprjname + "\\" + value;
                    x_node.Attributes.Append(attrInclude);

                    foreach (var item in p.Value.ChildNodes)
                    {
                        //Console.WriteLine(((XmlElement)item).Name + "   " + ((XmlElement)item).InnerText);

                        var z_node = dest.CreateElement(((XmlElement)item).Name, dest.DocumentElement.NamespaceURI);
                        z_node.InnerText = ((XmlElement)item).InnerText;
                        x_node.AppendChild(z_node);

                        //if (((XmlElement)item).Name.Equals("DependentUpon"))
                            //depOn = true;
                    }
                    //if (!depOn && !value.StartsWith("Resources\\"))
                    //{
                    //    var yy_depOn = dest.CreateElement("DependentUpon", dest.DocumentElement.NamespaceURI);
                    //    yy_depOn.InnerText = Path.ChangeExtension(value, ".cs");
                    //    x_node.AppendChild(yy_depOn);
                    //}
                    var yy_link = dest.CreateElement("Link", dest.DocumentElement.NamespaceURI);
                    yy_link.InnerText = value;
                    x_node.AppendChild(yy_link);

                    var xxx = get(dest, n_ode);
                    if (xxx != null)
                        xxx.AppendChild(x_node);
                    else
                    {
                        xxx = get(dest, null);//ultimo ItemGroup
                        if (xxx == null)
                        {
                            var y_node = dest.CreateElement("ItemGroup", dest.DocumentElement.NamespaceURI);
                            y_node.AppendChild(x_node);
                            dest.DocumentElement.AppendChild(y_node);
                        }
                        else
                            xxx.AppendChild(x_node);
                    }
                    Console.WriteLine("NF: " + attrInclude.Value + "   "+ aprjname);
                }
            }

#if false
            var nodes = orig.GetElementsByTagName("ItemGroup");
            if (nodes != null)
            {
                foreach (XmlElement i in nodes)
                {
                    foreach (var n_ode in new string[] { "Compile", "EmbeddedResource", "None", "WebReferenceUrl" })
                    {
                        foreach (XmlElement b in i.GetElementsByTagName(n_ode))
                        {
                            if (b.HasAttributes)
                            {
                                var attr = b.Attributes["Include"];
                                if (attr != null)
                                {
                                    if (n_ode.Equals("None") && attr.Value.Equals("app.config"))
                                        continue;

                                    //if ((n_ode.Equals("None") || n_ode.Equals("WebReferenceUrl")) && !projectOriginal.EndsWith("NFe.Components.csproj"))
                                    //  continue;

                                    Dictionary<string, string> opcoes = new Dictionary<string, string>();
                                    foreach (XmlElement chd in b.ChildNodes)
                                    {
                                        opcoes.Add(chd.Name, chd.InnerText);
                                    }
                                    updateprojeto(dest,
                                            folderDestino,
                                            b.Name, "..\\..\\" + Path.GetFileNameWithoutExtension(projectOriginal),
                                            attr.Value, opcoes);
                                }
                            }
                        }
                    }
                }
            }
#endif
            if (modificado)
            {
                Console.WriteLine("Writing");
                dest.Save(projectDestino + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
                dest.Save(projectDestino);
            }
        }

        static XmlNode get(XmlDocument dest, string key)
        {
            XmlNode result = null;
            foreach (XmlElement i in dest.GetElementsByTagName("ItemGroup"))
            {
                result = i;
                if (key != null)
                {
                    //key = "Compile", "EmbeddedResource", "None", "WebReferenceUrl"
                    foreach (XmlElement b in i.GetElementsByTagName(key))
                    {
                        return i;
                    }
                }
            }
            return result;
        }

#if false
        static bool considera(string linkname)
        {
            if (linkname.Equals("Properties\\AssemblyInfo.cs")) return false;
            if (linkname.Equals("Properties\\Settings.Designer.cs")) return false;
            if (linkname.Equals("Properties\\Settings.settings")) return false;
            if (linkname.Equals("Properties\\Resources.Designer.cs")) return false;
            if (linkname.Equals("Properties\\Resources.resx")) return false;
            if (linkname.StartsWith("System.")) return false;
            if (linkname.StartsWith("Microsoft.")) return false;
            //if (linkname.StartsWith("Web References")) return false;
            return true;
        }

        private static void updateprojeto(XmlDocument dest, string folderdest, string tipo, string filename, string linkname, Dictionary<string, string> opcoes)
        {
            if (!considera(linkname)) return;

            XmlElement node = null;
            bool found = false;

            foreach (var i1 in dest.ChildNodes)
            {
                if (i1 is XmlElement && ((XmlElement)i1).Name.Equals("Project"))
                {
                    foreach (var i2 in ((XmlElement)i1).ChildNodes)
                    {
                        if (i2 is XmlElement && ((XmlElement)i2).Name.Equals("ItemGroup"))
                        {
                            if (node == null)
                                node = (XmlElement)i2;

                            foreach (var i3 in ((XmlElement)i2).ChildNodes)
                            {
                                if (i3 is XmlElement)
                                {
                                    var attr = ((XmlElement)i3).Attributes["Include"];
                                    if (attr != null)
                                    {
                                        if (!considera(attr.Value)) continue;

                                        if (attr.Value.Equals(linkname))
                                        {
                                            ((XmlElement)i2).RemoveChild((XmlNode)i3);
                                            modificado = true;
                                        }
                                        if (attr.Value.Equals(filename + @"\" + linkname))
                                            found = true;
                                    }
                                }
                            }
                            if (((XmlElement)i2).ChildNodes.Count == 0 && i2 != node)
                            {
                                ((XmlElement)i1).RemoveChild((XmlNode)i2);
                                modificado = true;
                            }
                        }
                    }
                }
            }
            if (!found)
            {
                //Console.WriteLine("<" + tipo + " Include=\"" + filename + @"\" + linkname + "\"><Link>" + linkname + "</Link>" + "</" + tipo + ">");

                //if (!Directory.Exists(Path.Combine(folderdest, Path.GetDirectoryName(linkname))))
                //{
                //    ///
                //    /// cria a pasta no projeto destino
                //    /// 
                //    Console.WriteLine();
                //    Console.WriteLine(folderdest);
                //    Console.WriteLine(linkname);
                //    Console.WriteLine(Path.GetDirectoryName(linkname));
                //    Console.WriteLine();

                //    throw new Exception(Path.Combine(folderdest, Path.GetDirectoryName(linkname)));
                //    Directory.CreateDirectory(Path.Combine(folderdest, Path.GetDirectoryName(linkname)));
                //}

                XmlNode r = dest.CreateElement(tipo, dest.DocumentElement.NamespaceURI);
                XmlElement link = null;
                if (tipo != "WebReferenceUrl" && tipo != "Web References")
                {
                    link = dest.CreateElement("Link", dest.DocumentElement.NamespaceURI);
                    link.InnerText = linkname;
                    node.AppendChild(r);
                }

                var depon = false;
                if (linkname.EndsWith(".resx"))
                {
                    if (!opcoes.ContainsKey("DependentUpon"))
                    {
                        XmlElement DependentUpon = dest.CreateElement("DependentUpon", dest.DocumentElement.NamespaceURI);
                        DependentUpon.InnerText = Path.GetFileNameWithoutExtension(linkname) + ".cs";
                        r.AppendChild(DependentUpon);
                        depon = true;
                    }
                }

                if (opcoes.Count > 0)
                {
                    foreach (KeyValuePair<string,string> p in opcoes)
                    {
                        if (p.Key.Equals("DependentUpon") && depon) continue;

                        XmlElement n = dest.CreateElement(p.Key, dest.DocumentElement.NamespaceURI);
                        n.InnerText = p.Value;
                        r.AppendChild(n);
                    }
                }

                XmlAttribute attrInclude = dest.CreateAttribute("Include");
                if (link == null)//tipo == "WebReferenceUrl" || tipo == "Web References")
                    attrInclude.Value = linkname;
                else
                    attrInclude.Value = filename + "\\" + linkname;
                r.Attributes.Append(attrInclude);

                if (link != null)
                    r.AppendChild(link);

                modificado = true;
            }
        }
#endif
    }
}
