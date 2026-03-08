using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;

namespace Global.Funciones
{
	public class Mail
	{


		/// <summary>
		/// FromCredencialUsuario: Casilla de Correo que se logea en el SMTP
		/// </summary>
		public string FromCredencialUsuario { get; set; }

		/// <summary>
		/// FromCredencialPassword: Password de la casilla de Correo que se logea en el SMTP
		/// </summary>
		public string FromCredencialPassword { get; set; }

		/// <summary>
		/// FromUsuario: Casilla de Correo que envía el mail
		/// </summary>
		public string FromUsuario { get; set; }

		/// <summary>
		/// FromDetalle: Detalle de la casilla de correo que envía el mail
		/// </summary>
		public string FromDetalle { get; set; }

		/// <summary>
		/// ToDirecciones: Direcciones que tienen que estar delimitadas por ","
		/// </summary>
		public string ToDirecciones { get; set; }

		/// <summary>
		/// ToReply: Direccion de reenvio ","
		/// </summary>
		public string ToReply { get; set; }



		/// <summary>
		/// Puerto: Puerto del Servidor de Correo SMTP
		/// </summary>
		public int Puerto { get; set; }

		/// <summary>
		/// Host: Direccion SMTP (Ej. smtp.gmail.com)
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// SSLActivado: Verdadero o Falso
		/// </summary>
		public bool SSLActivado { get; set; }

		/// <summary>
		/// MensajeSubject: El Subject del mensaje
		/// </summary>
		public string MensajeSubject { get; set; }


		/// <summary>
		/// Mensaje": Mensaje que se va a enviar por mail
		/// </summary>
		public string MensajeCuerpo { get; set; }


		public void EnviarMail()
		{			
			try
			{

			
				MailMessage message = new MailMessage();
				message.From = new MailAddress(FromUsuario, FromDetalle, System.Text.Encoding.UTF8);
				message.IsBodyHtml = false;

				String[] addr = this.ToDirecciones.Split(',');
				Byte i;
				for (i = 0; i < addr.Length; i++)
					message.To.Add(addr[i].Trim());

				message.Subject = MensajeSubject;
				message.Body = MensajeCuerpo;

				message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
				//if (ToReply != String.Empty)
				//	message.ReplyTo = new MailAddress(ToReply);

				SmtpClient SmtpServer = new SmtpClient(this.Host, this.Puerto);
				SmtpServer.UseDefaultCredentials = false;
				SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
				SmtpServer.EnableSsl = this.SSLActivado;
				SmtpServer.Credentials = new System.Net.NetworkCredential(FromCredencialUsuario, FromCredencialPassword);
				SmtpServer.Send(message);

			}
			catch (SmtpException ex)
			{

				System.Windows.Forms.MessageBox.Show(ex.Message);
			}
		}

	}


}
