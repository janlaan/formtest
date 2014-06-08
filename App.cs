using System;
using Xamarin.Forms;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Formtest
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			Layout l = parseThing ();
			return new ContentPage { 
				Content = l
					/*new Label {
					Text = "Hello, Forms !",
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				},*/
			};
		}

		private static Layout parseThing() {
			WebClient w = new WebClient();
			string str = w.DownloadString ("https://noveria.nl/apitest.html");
			//string str = {"fields":[{"label":"Maak foto's van het ongeluk","field_type":"photo","required":true,"field_options":{"description":"let vooral op de pinda's"},"cid":"c6"},{"label":"Aard v/h ongeluk","field_type":"text","required":true,"field_options":{"size":"small"},"cid":"c10"},{"label":"Veroorzaker","field_type":"text","required":true,"field_options":{"size":"medium","minlength":"ape","maxlength":"nuts"},"cid":"c17"},{"label":"Handtekening","field_type":"signature","required":true,"field_options":{},"cid":"c2"}]}
			var parsed = Newtonsoft.Json.Linq.JObject.Parse (str);
			IList<View> childs = new List<View> ();

			ListView lv = new ListView ();
			foreach(JObject field in parsed["fields"]) {
				Label lbl = genLabel (field);
				View element = genInput (field);
				if (element != null) {
					childs.Add (lbl);
					childs.Add (element);
				}
			}


			StackLayout layout = new StackLayout {
				Children = {}
				
				
			};
			foreach (View vw in childs) {
				layout.Children.Add (vw);
			}
			Button submitButton = new Button {
				Text = "Submit"
			};
			submitButton.Clicked += (sender, EventArgs) => {
				List<Object> listo = new List<Object>();
				foreach(View gv in childs) {
					if(gv is DatePicker)
						listo.Add(((DatePicker) gv).Date);
					if(gv is Entry)
						listo.Add(((Entry) gv).Text);
					if(gv is Button)
						listo.Add(((Button) gv).Text);

				}
				string outstring = Newtonsoft.Json.JsonConvert.SerializeObject(listo);
				Console.WriteLine(outstring);
			};

			layout.Children.Add (submitButton);

			var v = new ScrollView {
				Content = layout
			};

			return v;
		}

		static Label genLabel(JObject lal) {
			return new Label {
				Text = (string)lal ["label"],
				BackgroundColor = Color.Blue
			};
		}

		static View genInput(JObject input) {
			switch ((string) input ["field_type"]) {
			case "photo":
				return new DatePicker ();
			case "text":
				return new Entry ();
			case "signature":
				return new Button ();
			default:
				return null;
			}
		}
	}
}

