Module Module1
    Sub Main()
        Dim d As String = Application.StartupPath
        If System.Environment.GetCommandLineArgs.Length > 1 Then
            Try
                d = System.Environment.GetCommandLineArgs(1)
            Catch
                MsgBox(Err.Description & "Source: " & Err.Source & "Line: " + Err.Erl & vbNewLine & "ixexpot got a problem with: " & System.Environment.GetCommandLineArgs(1))
            End Try
        End If

        For Each f As String In System.IO.Directory.GetFiles(d, "*.igx", IO.SearchOption.AllDirectories)
            ExportXML(f)
        Next
    End Sub
    Private Sub ExportXML(ByVal fname As String)
        Console.WriteLine("Exporting: " & fname)
        Dim app As Object = CreateObject("iGrafx.Application.8")
        Dim igxdoc As Object = app.Documents.Open(fname)

        Dim xdoc As New XDocument

        Dim doc As XElement = New XElement("Document")
        xdoc.Add(doc)

        Dim diags As XElement = New XElement("Diagrams")
        doc.Add(diags)

        For Each d As Object In igxdoc.Diagrams
            Dim diag As New XElement("Diagram")
            diags.Add(diag)
            diag.Add(New XAttribute("Name", d.Name))
            Console.WriteLine(d.name)
            diag.Add(New XAttribute("DiagramID", d.DiagramID))
            diag.Add(New XAttribute("DiagramType", d.DiagramType.SingularName))
            ' diag.Add(New XAttribute("EnterpriseXML", d.EnterpriseXML))
            Dim ent As XElement
            ent = XElement.Parse(d.EnterpriseXML)
            diag.Add(ent)

            diag.Add(New XAttribute("FullName", d.FullName))
            diag.Add(New XAttribute("Height", d.Height))
            diag.Add(New XAttribute("Width", d.Width))
            diag.Add(New XAttribute("PageHeight", d.PageLayout.PageHeight))
            diag.Add(New XAttribute("PageWidth", d.PageLayout.PageWidth))
            Dim layers As New XElement("Layers")
            diag.Add(layers)
            For Each l As Object In d.Layers
                Dim layer As New XElement("Layer")
                layers.Add(layer)
                layer.Add(New XAttribute("Index", l.Index))
                layer.Add(New XAttribute("Locked", l.Locked))
                layer.Add(New XAttribute("Name", l.Name))
                layer.Add(New XAttribute("Printable", l.Printable))
                layer.Add(New XAttribute("Visible", l.Visible))
            Next
            Dim pages As New XElement("Pages")
            diag.Add(pages)
            pages.Add(New XAttribute("PagesAcross", d.Pages.PagesAcross))
            pages.Add(New XAttribute("PagesDown", d.Pages.PagesDown))
            For Each p As Object In d.Pages
                Dim page As New XElement("Page")
                pages.Add(page)
                page.Add(New XAttribute("Bottom", p.Bottom))
                page.Add(New XAttribute("Height", p.Height))
                page.Add(New XAttribute("Left", p.Left))
                page.Add(New XAttribute("Right", p.Right))
                page.Add(New XAttribute("Top", p.Top))
                page.Add(New XAttribute("Width", p.Width))
                If Not p.ObjectRange Is Nothing Then
                    Dim rng As New XElement("ObjectRange")
                    page.Add(rng)
                    rng.Add(New XAttribute("Bottom", p.ObjectRange.Bottom))
                    rng.Add(New XAttribute("Height", p.ObjectRange.Height))
                    rng.Add(New XAttribute("Left", p.ObjectRange.Left))
                    rng.Add(New XAttribute("Right", p.ObjectRange.Right))
                    rng.Add(New XAttribute("Top", p.ObjectRange.Top))
                    rng.Add(New XAttribute("Width", p.ObjectRange.Width))
                    rng.Add(New XAttribute("CenterX", p.ObjectRange.CenterX))
                    rng.Add(New XAttribute("CenterY", p.ObjectRange.CenterY))
                End If
            Next
            Dim departments As New XElement("Departments")
            diag.Add(departments)
            For Each dd As Object In d.Departments
                Dim dep As New XElement("Department")
                departments.Add(dep)
                dep.Add(New XAttribute("DepartmentName", dd.DepartmentName))
                dep.Add(New XAttribute("DepartmentIndex", dd.DepartmentIndex))
                dep.Add(New XAttribute("CanonicalName", dd.CanonicalName))
                If Not dd.ParentDepartment Is Nothing Then
                    dep.Add(New XAttribute("ParentDepartment", dd.ParentDepartment.DepartmentName))
                End If
                If Not dd.PermanentDepartment Is Nothing Then
                    dep.Add(New XAttribute("PermanentDepartment", dd.PermanentDepartment.DepartmentName))
                End If
                dep.Add(New XAttribute("Text", dd.Text))
                dep.Add(New XAttribute("Size", dd.Size))
                dep.Add(New XAttribute("MinimumSize", dd.MinimumSize))
                Console.WriteLine(dd.DepartmentName)
            Next
            Dim objects As New XElement("DiagramObjects")
            diag.Add(objects)
            If Not d.DiagramObjects.ObjectRange Is Nothing Then
                Dim rng As New XElement("ObjectRange")
                diag.Add(rng)
                rng.Add(New XAttribute("Bottom", d.DiagramObjects.ObjectRange.Bottom))
                rng.Add(New XAttribute("Height", d.DiagramObjects.ObjectRange.Height))
                rng.Add(New XAttribute("Left", d.DiagramObjects.ObjectRange.Left))
                rng.Add(New XAttribute("Right", d.DiagramObjects.ObjectRange.Right))
                rng.Add(New XAttribute("Top", d.DiagramObjects.ObjectRange.Top))
                rng.Add(New XAttribute("Width", d.DiagramObjects.ObjectRange.Width))
                rng.Add(New XAttribute("CenterX", d.DiagramObjects.ObjectRange.CenterX))
                rng.Add(New XAttribute("CenterY", d.DiagramObjects.ObjectRange.CenterY))
            End If

            For Each dd As Object In d.DiagramObjects
                Dim obj As New XElement("DiagramObject")
                objects.Add(obj)
                obj.Add(New XAttribute("ID", dd.ID))
                obj.Add(New XAttribute("Type", dd.Type))
                obj.Add(New XAttribute("Diagram", dd.Diagram.Name))
                obj.Add(New XAttribute("Bottom", dd.Bottom))
                obj.Add(New XAttribute("Height", dd.Height))
                obj.Add(New XAttribute("Left", dd.Left))
                obj.Add(New XAttribute("Right", dd.Right))
                obj.Add(New XAttribute("Top", dd.Top))
                obj.Add(New XAttribute("Width", dd.Width))
                obj.Add(New XAttribute("CenterX", dd.CenterX))
                obj.Add(New XAttribute("CenterY", dd.CenterY))
                obj.Add(New XAttribute("Layer", dd.Layer.Name))

                Try
                    obj.Add(New XAttribute("ObjectName", dd.ObjectName))
                    Console.WriteLine(fname & " Object " & dd.ID & " " & dd.Type & " " & dd.ObjectName)
                Catch
                End Try
                Try
                    If Not dd.Phase Is Nothing Then
                        obj.Add(New XAttribute("PhaseIndex", dd.Phase.PhaseIndex))
                        obj.Add(New XAttribute("PhaseName", dd.Phase.PhaseName))
                    End If
                Catch
                End Try
                Try
                    Dim dda As iGrafx4.Activity = dd.AsType("iGrafx.Activity")
                    Select Case dda.BPMNType
                        Case iGrafx4.IxBPMNType.ixBPMNActivity, iGrafx4.IxBPMNType.ixBPMNUnspecified
                            obj.Add(New XAttribute("BPMNType", dda.BPMNType))
                            obj.Add(New XAttribute("BPMNSubType", dda.BPMNSubType))
                            obj.Add(New XAttribute("BPMNSubTypeProperty", dda.BPMNSubTypeProperty))
                            obj.Add(New XAttribute("Capacity", dda.Capacity.Value))
                            obj.Add(New XAttribute("Duration", dda.Duration.Value))
                            obj.Add(New XAttribute("DurationTimeUnit", dda.DurationTimeUnit))
                            obj.Add(New XAttribute("FixedCost", dda.FixedCost.Value))
                            obj.Add(New XAttribute("Overtime", dda.Overtime))
                            obj.Add(New XAttribute("RepeatType", dda.RepeatType))
                            obj.Add(New XAttribute("RepeatCondition", dda.RepeatCondition.Value))
                            obj.Add(New XAttribute("RepeatCount", dda.RepeatCount.Value))
                            ' obj.Add(New XAttribute("SubprocessType ", dda.SubprocessType))
                            obj.Add(New XAttribute("TaskPerformerType", dda.TaskPerformerType))
                            'obj.Add(New XAttribute("WaitTime ", dda.WaitTime.ToString))
                            obj.Add(New XAttribute("TaskType", dda.TaskType))
                            obj.Add(New XAttribute("ValueClass", dda.ValueClass))
                            Dim act As XElement
                            act = XElement.Parse(dda.IGXML)
                            obj.Add(act)
                        Case iGrafx4.IxBPMNType.ixBPMNEvent
                        Case iGrafx4.IxBPMNType.ixBPMNGateway
                        Case Else

                    End Select
                Catch ex As Exception

                End Try

                Try
                    If Not dd.Shape Is Nothing Then
                        Dim s As Object = dd.Shape
                        Dim shp As New XElement("Shape")
                        obj.Add(shp)
                        shp.Add(New XAttribute("Text", s.Text))
                        ' shp.Add(New XAttribute("EnterpriseXML", s.EnterpriseXML))
                        Dim entshp As XElement
                        entshp = XElement.Parse(s.EnterpriseXML)
                        obj.Add(entshp)
                        shp.Add(New XAttribute("IsCrossDepartment", s.IsCrossDepartment))
                        shp.Add(New XAttribute("IsDecision", s.IsDecision))
                        shp.Add(New XAttribute("IsStartPoint", s.IsStartPoint))
                        ' shp.Add(New XAttribute("LineColor", s.LineColor))
                        shp.Add(New XAttribute("ShapeClass", s.ShapeClass.Name))
                        Try
							shp.Add(New XAttribute("TopDepartment", s.TopDepartment.DepartmentName))
                        catch
						end try
                        Try
						shp.Add(New XAttribute("BottomDepartment", s.BottomDepartment.DepartmentName))
                        catch
						end try
                        Try
                            shp.Add(New XAttribute("Note", s.Note.Text))
                        Catch
                        End Try
                        shp.Add(New XAttribute("AutoGrow", s.AutoGrow))
                        shp.Add(New XAttribute("BackColor", s.BackColor))
                        shp.Add(New XAttribute("FillColor", s.FillColor))
                        shp.Add(New XAttribute("FillType", s.FillType))
                        shp.Add(New XAttribute("LineColor", s.LineColor))
                        shp.Add(New XAttribute("LineEffect", s.LineEffect))
                        shp.Add(New XAttribute("LineStyle", s.LineStyle))
                        shp.Add(New XAttribute("LineWidth", s.LineWidth))
                        shp.Add(New XAttribute("ShadowColor", s.ShadowColor))
                        shp.Add(New XAttribute("ShadowDepth", s.ShadowDepth))
                        shp.Add(New XAttribute("ShadowType", s.ShadowType))


                        'If Not s.TextGraphicObject Is Nothing Then
                        '    Dim t As Object = s.TextGraphicObject
                        '    Dim textgraphicobject As New XElement("TextGraphicObject")
                        '    shp.Add(textgraphicobject)

                        '    If Not t.FillFormat Is Nothing Then
                        '        Dim f As Object = t.FillFormat
                        '        Dim fillformat As New XElement("FillFormat")
                        '        textgraphicobject.Add(fillformat)

                        '        fillformat.Add(New XAttribute("BackColor", f.BackColor))
                        '        fillformat.Add(New XAttribute("FillColor", f.FillColor))
                        '        fillformat.Add(New XAttribute("FillType", f.FillType))
                        '    End If
                        'End If
                        If s.OutputPaths.Count > 0 Then
                            Dim outputpaths As New XElement("OutputPaths")
                            shp.Add(outputpaths)
                            For Each ll As Object In s.OutputPaths
                                Dim out As New XElement("OutputPath")
                                outputpaths.Add(out)
                                out.Add(New XAttribute("OutputID", ll.Destination.DiagramObject.ID))
                                out.Add(New XAttribute("OutputCase", ll.DecisionCaseIndex))
                                out.Add(New XAttribute("Name", ll.Name))
                                out.Add(New XAttribute("OutputText", ll.Destination.Text))
                                out.Add(New XAttribute("InputID", ll.Source.DiagramObject.ID))
                                If ll.DecisionCaseText IsNot Nothing Then
                                    out.Add(New XAttribute("DecisionCaseText", ll.DecisionCaseText))
                                End If
                          Next
                        End If
                        If s.DecisionCases.count > 0 Then
                            Dim decisions As New XElement("DecisionCases")
                            shp.Add(decisions)
                            Dim idx As Integer = 1
                            For Each ll As Object In s.DecisionCases
                                Dim des As New XElement("DecisionCase")
                                decisions.Add(des)
                                'des.Add(New XAttribute("OutputID", ll.Destination.DiagramObject.ID))
                                des.Add(New XAttribute("idx", idx))
                                des.Add(New XAttribute("Case", ll.Name))
                                des.Add(New XAttribute("Exclusive", ll.Exclusive))
                                des.Add(New XAttribute("Percent", ll.Percent))
                                des.Add(New XAttribute("StartArrow", ll.StartArrow))
                                idx += 1
                            Next
                        End If
                        If s.Links.count > 0 Then
                            Dim links As New XElement("Links")
                            shp.Add(links)
                            For Each ll As Object In s.Links
                                Dim lnk As New XElement("Link")
                                links.Add(lnk)
                                lnk.Add(New XAttribute("Key", ll.Key))
                                lnk.Add(New XAttribute("Description", ll.Description))
                                lnk.Add(New XAttribute("IsSubProcess", ll.IsSubProcess))
                                ' lnk.Add(New XAttribute("AccumulateData", ll.AccumulateData))
                                'lnk.Add(New XAttribute("IsSubProcess", ll.IsSubProcess))
                                'lnk.Add(New XAttribute("StartPointName", ll.StartPointName))
                                lnk.Add(New XAttribute("Target", ll.Target))
                                lnk.Add(New XAttribute("TargetRelative", ll.TargetRelative))
                                lnk.Add(New XAttribute("TargetShapeID", ll.TargetShapeID))
                                lnk.Add(New XAttribute("Type", ll.Type))
                            Next
                        End If
                    End If
                Catch
                End Try
                Dim custvalso As XElement = New XElement("CustomDataValues")
                For Each d1 As Object In dd.CustomDataValues
                    If Len(d1.FormattedValue) > 0 And d1.FormattedValue <> "0.0" Then
                        Dim dd1 As New XElement("CustomDataValue")
                        custvalso.Add(dd1)
                        dd1.Add(New XAttribute("Name", d1.Name))
                        If Not d1.CustomDataDefinition Is Nothing Then
                            dd1.Add(New XAttribute("CustomDataDefinition", d1.CustomDataDefinition.ID))
                        End If
                        dd1.Add(New XAttribute("Day", d1.Day))
                        dd1.Add(New XAttribute("FormattedValue", d1.FormattedValue))
                        dd1.Add(New XAttribute("Hours", d1.Hours))
                        dd1.Add(New XAttribute("identity", d1.identity))
                        dd1.Add(New XAttribute("IsEmpty", d1.IsEmpty))
                        dd1.Add(New XAttribute("Minutes", d1.Minutes))
                        dd1.Add(New XAttribute("Month", d1.Month))
                        Try
                            dd1.Add(New XAttribute("RawValue", d1.RawValue))
                        Catch
                        End Try
                        dd1.Add(New XAttribute("Seconds", d1.Seconds))
                        dd1.Add(New XAttribute("TMUs", d1.TMUs))
                        dd1.Add(New XAttribute("Type", d1.Type))
                        dd1.Add(New XAttribute("Value", d1.Value))
                        Try
                            dd1.Add(New XAttribute("Weeks", d1.Weeks))
                        Catch
                        End Try
                        Try
                            dd1.Add(New XAttribute("Year", d1.Year))
                        Catch
                        End Try
                    End If
                Next
                If custvalso.Elements("CustomDataValue").Count > 0 Then
                    obj.Add(custvalso)
                End If

            Next
        Next
        Dim custdefs As XElement = New XElement("CustomDataDefinitions")
        doc.Add(custdefs)
        For Each d As Object In igxdoc.CustomDataDefinitions
            Dim dd As New XElement("CustomDataDefinition")
            custdefs.Add(dd)
            dd.Add(New XAttribute("Name", d.Name))
            dd.Add(New XAttribute("Description", d.Description))
            dd.Add(New XAttribute("ID", d.ID))
            dd.Add(New XAttribute("identity", d.identity))
            ' dd.Add(New XAttribute("IsMultiValuePickList", d.IsMultiValuePickList))
            'dd.Add(New XAttribute("PickListMembers", d.PickListMembers))
            'dd.Add(New XAttribute("AccumulationMethod", d.AccumulationMethod))
            ' dd.Add(New XAttribute("DataFormat", d.DataFormat))
        Next
        Dim custvals As XElement = New XElement("CustomDataValues")
        doc.Add(custvals)
        For Each d As Object In igxdoc.CustomDataValues
            Dim dd As New XElement("CustomDataValue")
            custvals.Add(dd)
            dd.Add(New XAttribute("Name", d.Name))
            If Not d.CustomDataDefinition Is Nothing Then
                dd.Add(New XAttribute("CustomDataDefinition", d.CustomDataDefinition.ID))
            End If
            dd.Add(New XAttribute("Day", d.Day))
            dd.Add(New XAttribute("FormattedValue", d.FormattedValue))
            dd.Add(New XAttribute("Hours", d.Hours))
            dd.Add(New XAttribute("identity", d.identity))
            dd.Add(New XAttribute("IsEmpty", d.IsEmpty))
            dd.Add(New XAttribute("Minutes", d.Minutes))
            dd.Add(New XAttribute("Month", d.Month))
            Try
                dd.Add(New XAttribute("RawValue", d.RawValue))
            Catch
            End Try
            dd.Add(New XAttribute("Seconds", d.Seconds))
            dd.Add(New XAttribute("TMUs", d.TMUs))
            dd.Add(New XAttribute("Type", d.Type))
            dd.Add(New XAttribute("Value", d.Value))
            Try
                dd.Add(New XAttribute("Weeks", d.Weeks))
            Catch
            End Try
            Try
                dd.Add(New XAttribute("Year", d.Year))
            Catch
            End Try

        Next
        Console.WriteLine("Saving to " & fname.Replace("igx", "xml"))
        xdoc.Save(fname.Replace("igx", "xml"))
        Console.WriteLine(fname & " is done.")
		app.closeall()
    End Sub

End Module
