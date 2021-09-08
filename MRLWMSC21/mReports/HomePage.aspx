 <%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="MRLWMSC21.mReports.ChartControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    
   
                                            <table cellpadding="0" cellspacing="0" border="0" align="left" >
											
                                                <tr>
                                                    <td></td>
                                                    <td colspan="1" align="left">
                                                        <span style="font-size:13pt; font-family:Arial; color:currentColor;font-weight: bold">
                                                           &nbsp;&nbsp; Warehouse
                                                        </span>
                                                    </td>
                                                </tr>
												<tr>
												    <td class="box_left">&nbsp;</td>
												    <td valign="top" colspan="2" width="860">
												    <asp:Label id="lblResults"   CssClass="ErrorMsg"   runat="server" /> 
												       <asp:Literal ID="ltBodyContent" runat="server" />
												       
    											       <asp:Panel runat="server" ID="pnlCharts" Visible="false">
    											      
    											        <table cellpadding="1" cellspacing="1" border="0" >
												    <tr>
												    <td >
											      <asp:Chart ID="ChtInOutActivities" runat="server" Width="400px" Height="260px" BorderDashStyle="Solid" BackSecondaryColor="Cornsilk" BackGradientStyle="DiagonalRight"  BorderWidth="2" backcolor="#D3DFF0" BorderColor="26, 59, 105"  Palette="Pastel" > 
                                                         <Legends>
                                                            <asp:Legend Docking="Bottom" LegendStyle="Row" Name="Legend1" Font ="Calibri, 8pt style=Bold">
                                                            </asp:Legend>
                                                        </Legends>
                                                    
                                                        <Titles>
                                                          <asp:Title Name="Title1" Text="Inbound Vs Outbound " 
                                                                     Alignment="TopLeft" Font="Arial, 11pt, style=Bold" Docking="Top"  ForeColor="220,64,64,64" />
                                                                     
                                                           <asp:Title Name="Title2" Text="(warehouse wise  for last 10 days)" 
                                                                     Alignment="TopLeft"  Font="Arial, 8pt, style=Bold" Docking="Top" ForeColor="220,64,64,64" />
                                                                     
                                                       </Titles>
                                                         <borderskin skinstyle="Raised"></borderskin>
                                                       
                                                       <Series >
                                                               <asp:Series ChartType="Column"  Name="Inbound" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.6"  
                                                               ChartArea="MainChartArea"   Font="Arial,6pt" 
                                                               Legend="Legend1" Color="210,148,199,94"  ></asp:Series> 
                                                               
                                                                <asp:Series ChartType="Column"   Name="Outbound" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.6"  
                                                               ChartArea="MainChartArea"   Font="Arial,6pt" 
                                                               Legend="Legend1" Color="230,254,208,34" ></asp:Series> 
                                                        </Series> 
                                                        
                                                       <ChartAreas> 
                                                       
                                                         <asp:ChartArea Name="MainChartArea"  IsSameFontSizeForAllAxes="false"  BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                                                <axisy linecolor="64, 64, 64, 64" IsLabelAutoFit="False" >
                                                                    <labelstyle  Font ="Calibri, 8pt style=bold" />
                                                                    <majorgrid linecolor="64, 64, 64, 64" />
                                                                </axisy>
                                                                
                                                                <axisx linecolor="64, 64, 64, 64" IsLabelAutoFit="False" Interval="1" >
                                                                    <labelstyle  Font ="Calibri, 8pt style=bold"   />
                                                                    <majorgrid linecolor="64, 64, 64, 64" />
                                                                </axisx>
                                                        </asp:ChartArea>
                                                        
                                                       </ChartAreas> 
                                                    </asp:Chart>
												    </td>	
                                                        
                                              <td valign="top">  
												    
												    <asp:Chart ID="chtInboundActivities" runat="server" Width="330px" Height="260px" BorderDashStyle="Solid" BackSecondaryColor="Cornsilk" BackGradientStyle="DiagonalRight"  BorderWidth="2" backcolor="#D3DFF0" BorderColor="26, 59, 105"  Palette="Pastel" > 
                                                        <Legends>
                                                            <asp:Legend Docking="Bottom" LegendStyle="Row" Name="Legend1" Font ="Calibri, 8pt style=Bold" >
                                                            </asp:Legend>
                                                        </Legends>
                                                     
                                                        <Titles>
                                                               
                                                                  <asp:Title Name="Title1" Text="Inbound Statistics" 
                                                                     Alignment="TopLeft" Font="Arial, 11pt, style=Bold" Docking="Top"  ForeColor="220,64,64,64" /> 
                                                                
                                                                <asp:Title Name="Title2" Text="(for last 5 days)" 
                                                                     Alignment="TopLeft"  Font="Arial, 8pt, style=Bold" Docking="Top" ForeColor="220,64,64,64" />

                                                        </Titles>
                                                        
                                                       <borderskin skinstyle="Emboss"></borderskin>

                                                       <Series > 
                                                               <asp:Series ChartType="Column"   Name="Received" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.4"  
                                                               ChartArea="MainChartArea"   Font ="Arial, 8.25pt"
                                                                 Legend="Legend1" Color="210,148,199,94"   ></asp:Series> 
                                                                 
                                                               <asp:Series ChartType="Column"   Name="Verified" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.4"  
                                                               ChartArea="MainChartArea"   Font ="Arial, 8.25pt" 
                                                               Legend="Legend1" Color="230,254,208,34" ></asp:Series> 
                                                                 
                                                                <asp:Series ChartType="Column"   Name="GRNDone" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.4"  
                                                               ChartArea="MainChartArea"   Font="Arial,6pt" 
                                                              Legend="Legend1"  Color="Steelblue"  ></asp:Series> 
                                                                                                                      
                                                       </Series> 
                                                        
                                                       <ChartAreas> 
                                                             <asp:ChartArea Name="MainChartArea" IsSameFontSizeForAllAxes="true"  BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                                                <area3dstyle Enable3D="true" Rotation="15" perspective="2" Inclination="15" IsRightAngleAxes="False" wallwidth="0" ></area3dstyle>
                                                                <axisy linecolor="64, 64, 64, 64"  >
                                                                      <labelstyle  Font ="Calibri, 8pt style=bold"  />
                                                                      <majorgrid linecolor="64, 64, 64, 64" />
                                                                </axisy>
                                                          
                                                                <axisx linecolor="64, 64, 64, 64"   Interval="1" >
                                                                    <labelstyle  Font ="Calibri, 8pt style=bold" IsEndLabelVisible="false" Format="dd/MM" />
                                                                    <majorgrid linecolor="64, 64, 64, 64" />
                                                                </axisx>
                                                            </asp:ChartArea>

                                                       </ChartAreas> 
                                                       
                                                    </asp:Chart>
                 
												     </td>		
                                                        
                                                         <td valign="top"> 
												     
												     
												    <asp:Chart ID="chtOutboundAcivities" runat="server" Width="330px" Height="260px" BorderDashStyle="Solid" BackSecondaryColor="Cornsilk" BackGradientStyle="DiagonalRight"  BorderWidth="2" backcolor="#D3DFF0" BorderColor="26, 59, 105"  Palette="Pastel" > 
                                                    
                                                        <Legends>
                                                            <asp:Legend Docking="Bottom" LegendStyle="Row"  Name="Legend1" Font ="Calibri, 8pt style=Bold">
                                                            </asp:Legend>
                                                        </Legends>
                                                    
                                                        <Titles>
                   
                                                                  <asp:Title Name="Title1" Text="Outbound Statistics" 
                                                                     Alignment="TopLeft" Font="Arial, 11pt, style=Bold" Docking="Top"  ForeColor="220,64,64,64" />
                                                                
                                                                <asp:Title Name="Title2" Text="(for last 5 days)" 
                                                                     Alignment="TopLeft"  Font="Arial, 8pt, style=Bold" Docking="Top" ForeColor="220,64,64,64" />

                                                       </Titles>
                                                       <borderskin skinstyle="Emboss"></borderskin>

                                                       <Series > 
                                                              
                                                             <asp:Series ChartType="Column"   Name="Orders" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.4"  
                                                               ChartArea="MainChartArea"   Font ="Calibri, 8.25pt"
                                                                 Legend="Legend1" Color="210,148,199,94"   ></asp:Series> 
                                                                 
                                                                  <asp:Series ChartType="Column"   Name="PGIDone" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.4"  
                                                               ChartArea="MainChartArea"   Font ="Calibri, 8.25pt" 
                                                               Legend="Legend1" Color="230,254,208,34" ></asp:Series> 
                                                                 
                                                              <asp:Series ChartType="Column"   Name="Delivered with POD" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.4"  
                                                               ChartArea="MainChartArea"   Font="Arial,6pt" 
                                                              Legend="Legend1"  Color="Steelblue"  ></asp:Series> 
                                                                                                                      
                                                       </Series>  
                                                        
                                                       <ChartAreas> 
                                                         <asp:ChartArea Name="MainChartArea" IsSameFontSizeForAllAxes="true"  BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <area3dstyle Enable3D="true"  Rotation="15" perspective="2" Inclination="15" IsRightAngleAxes="False" wallwidth="0" ></area3dstyle>
                                       <axisy linecolor="64, 64, 64, 64"  >
                                      <labelstyle  Font ="calibri, 8pt style=bold" />
                                      <majorgrid linecolor="64, 64, 64, 64" />
                                </axisy>
                                
                                <axisx linecolor="64, 64, 64, 64"  Interval="1" >
                                <labelstyle  Font ="calibri, 8pt style=bold" IsEndLabelVisible="false"  Format="dd/MM"  />
                                <majorgrid linecolor="64, 64, 64, 64" />
                                </axisx>
                            </asp:ChartArea>

                                                       </ChartAreas> 
                                                       
                                                    </asp:Chart>
                 
												     
												     </td>								    
												 
												     
												    </tr>
                                                            <tr>
                                                                <td>
                                                                   <span style="font-size:13pt; font-family:Arial; color:currentColor; font-weight: bold">
                                                                       &nbsp;&nbsp; Manufacturing
                                                                   </span>
                                                                </td>
                                                            </tr>
                                                            <tr>

                                                               
                                                                <td >
											      <asp:Chart ID="chtRcvdAndConQtybyUser" runat="server" Width="400px" Height="260px" BorderDashStyle="Solid" BackSecondaryColor="Cornsilk" BackGradientStyle="DiagonalRight"  BorderWidth="2" backcolor="#D3DFF0" BorderColor="26, 59, 105"  Palette="Pastel" > 
                                                         <Legends>
                                                            <asp:Legend Docking="Bottom" LegendStyle="Row" Name="Legend1" Font ="Calibri, 8pt style=Bold">
                                                            </asp:Legend>
                                                        </Legends>
                                                    
                                                        <Titles>
                                                          <asp:Title Name="Title1" Text="Received & Consumed Quantity for Job Order" 
                                                                     Alignment="TopLeft" Font="Arial, 11pt, style=Bold" Docking="Top"  ForeColor="220,64,64,64" />                                                                     
                                                          
                                                                     
                                                       </Titles>
                                                         <borderskin skinstyle="Raised"></borderskin>
                                                       
                                                       <Series >
                                                               <asp:Series ChartType="Column"  Name="ReceivedQuantity" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.6"  
                                                               ChartArea="MainChartArea"   Font="Arial,6pt" 
                                                               Legend="Legend1" Color="210,148,199,94"  ></asp:Series> 
                                                               
                                                                <asp:Series ChartType="Column"   Name="ConsumedQuantity" 
                                                               CustomProperties="DrawingStyle=Cylinder, PointWidth=0.6"  
                                                               ChartArea="MainChartArea"   Font="Arial,6pt" 
                                                               Legend="Legend1" Color="230,254,208,34" ></asp:Series> 
                                                        </Series> 
                                                        
                                                       <ChartAreas> 
                                                       
                                                         <asp:ChartArea Name="MainChartArea"  IsSameFontSizeForAllAxes="false"  BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                                                <axisy linecolor="64, 64, 64, 64" IsLabelAutoFit="False" >
                                                                    <labelstyle  Font ="Calibri, 8pt style=bold" />
                                                                    <majorgrid linecolor="64, 64, 64, 64" />
                                                                </axisy>
                                                                
                                                                <axisx linecolor="64, 64, 64, 64" IsLabelAutoFit="False" Interval="1" >
                                                                    <labelstyle  Font ="Calibri, 8pt style=bold"   />
                                                                    <majorgrid linecolor="64, 64, 64, 64" />
                                                                </axisx>
                                                        </asp:ChartArea>
                                                        
                                                       </ChartAreas> 
                                                    </asp:Chart>
												    </td>



                                                               <td>
                                                                      <asp:Chart ID="chtProductionOrderStatus" runat="server" Width="330px" Height="260px" BorderDashStyle="Solid" BackSecondaryColor="Cornsilk" BackGradientStyle="DiagonalRight"  BorderWidth="2" backcolor="#D3DFF0" BorderColor="26, 59, 105"  Palette="Pastel" > 
                                                                                <Titles>
                                                                                    <asp:Title Name="Title1" Text="Job Order Status" 
                                                                     Alignment="TopLeft" Font="Arial, 11pt, style=Bold" Docking="Top"  ForeColor="220,64,64,64" />
                                                                                </Titles>
                                                                                <Legends>
                                                                                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="false" Name="Default" LegendStyle="Table" Font ="calibri, 8pt style=bold"/>
                                                                                </Legends>
                                                                          <borderskin skinstyle="Emboss"></borderskin>
                                                                                <Series>
                                                                                    <asp:Series ChartType="Pie" Name="Default" ChartArea="MainChartArea" IsValueShownAsLabel="true"/>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea  Name="MainChartArea" IsSameFontSizeForAllAxes="true"  BorderColor="64, 64, 64, 64"  BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
                                                                                </ChartAreas>
                                                                         </asp:Chart>
                                                                      <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1" OnLoad="Chart1_Load">
                                                                          <Series>
                                                                              <asp:Series ChartType="Pie" Legend="Legend1" Name="Series1" XValueMember="count" YValueMembers="count">
                                                                              </asp:Series>
                                                                          </Series>
                                                                          <ChartAreas>
                                                                              <asp:ChartArea Name="ChartArea1">
                                                                              </asp:ChartArea>
                                                                          </ChartAreas>
                                                                          <Legends>
                                                                              <asp:Legend Name="Legend1">
                                                                              </asp:Legend>
                                                                          </Legends>
                                                                      </asp:Chart>
                                                                      <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MRLWMSC21_RTConnectionString %>" SelectCommand="sp_CHT_GetProductionorderStatus" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                                </td>

                                                                
                                                                
                                                            </tr>
												    
												    </table>
                                                  
    											       </asp:Panel>
    											       
												    </td>
												    <td class="box_right"> </td>
											    </tr>
											   
											    
                                        </table>


             
   

 </asp:Content>  
