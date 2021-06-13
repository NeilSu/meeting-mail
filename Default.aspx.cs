using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;
using Microsoft.Exchange.WebServices.Data;
using System.Data.Objects.DataClasses;

namespace MeetingMial
{
    public partial class _Default : System.Web.UI.Page

    {
        private static ExchangeService _service; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dtNow = DateTime.Parse(DateTime.Now.ToShortDateString());

                calStartDate.SelectedDate = dtNow;
                calEndDate.SelectedDate = dtNow;
  

            }

        }

        protected void btnAddEvent_Click(object sender, EventArgs e)
        {
            


            //SmtpClient sc = new SmtpClient("relay-b.asus.com.tw");
            //MailMessage msg = new MailMessage();

            //string myMailEncoding = "utf-8";


            try
            {

                 


               // throw new InvalidPluginExecutionException("ActivityParties: " + ((EntityReference)Organizers[0].Attributes["partyid"]).Name.ToString());


                DateTime dtNow = DateTime.Now;
               StringBuilder str = new StringBuilder();
                _service = new ExchangeService(ExchangeVersion.Exchange2010);
                _service.UseDefaultCredentials = true;
                _service.AutodiscoverUrl(txtMailFrom.Text);
                Appointment appointment = new Appointment(_service);
                appointment.Subject = txtEventSummary.Text;
                appointment.Body = txtEventDescription.Text;
               // appointment.Attachments.AddFileAttachment("123.docx");



                string[] MailA = txtMailTo.Text.Split(';');
                for (int i = 0; i < MailA.Length; i++)
                {
                    if (MailA[i].ToString() != "")
                    {
                        appointment.RequiredAttendees.Add(MailA[i].ToString());
                      //  msg.To.Add(new MailAddress(MailA[i].ToString()));
                    }
                }
             //   appointment.RequiredAttendees.Add(txtMailTo.Text);

     

                string Stime= ddlStartTime.SelectedValue;
                Stime = Stime.Substring(0, 2) + ":" + Stime.Substring(2, 2) + ":00";
                DateTime SDate = Convert.ToDateTime(calStartDate.SelectedDate.Year.ToString() + "/" + calStartDate.SelectedDate.Month.ToString() + "/" + calStartDate.SelectedDate.Day.ToString() +" " +Stime);

                appointment.Start = Convert.ToDateTime(SDate);

                string Etime = ddlStartTime.SelectedValue;
                Etime = Etime.Substring(0, 2) + ":" + Etime.Substring(2, 2) + ":00";

                DateTime EDate = Convert.ToDateTime(calEndDate.SelectedDate.Year.ToString() + "/" + calEndDate.SelectedDate.Month.ToString() + "/" + calEndDate.SelectedDate.Day.ToString() + " " + Etime);

                appointment.End = Convert.ToDateTime(EDate);
                //DayOfTheWeek dayOfTheWeek = (DayOfTheWeek)Enum.Parse(
                //typeof(DayOfWeek), appointment.Start.DayOfWeek.ToString());
                //appointment.Recurrence = new Recurrence.WeeklyPattern(
                //appointment.Start.Date,
                //1, // Repeat every 1 week 
                //dayOfTheWeek);
                appointment.Save(SendInvitationsMode.SendToAllAndSaveCopy);

                Response.Write("<script>alert('Success！')</script>");
            }
            catch(Exception ex)
            {

                Response.Write("<script>alert('" + ex.ToString() + "！')</script>");
            }
            

        }
        private string FormatDateTimeValue(int DateValue)
        {
            if (DateValue < 10)
                return "0" + DateValue.ToString();
            else
                return DateValue.ToString();
        }
    }
}
