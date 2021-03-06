﻿/*
  * Date: 9/14/2012
 * Time: 6:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;

namespace vlcPlugIn
{
	public class VLCHttpControl
	{
		//constructor
		public VLCHttpControl()
		{
		}
		
		private string _vlcHost;
		private string _vlcPort;	
		private static string REMOTE_FILE = "/requests/status.xml";
		private static string PLAY_COMMAND = "?command=pl_play";
		private static string STOP_COMMAND = "?command=pl_stop";
		private static string PAUSE_COMMAND = "?command=pl_pause";
		private string uriStr;
		private string uriPlayStr;
		private string uriPauseStr;
		private int _vlcID;
		private int _vlcStartID;
		
		
		//override ctor
		public VLCHttpControl(string vlcHost,string vlcPort, int vlcID,int vlcStartID){
			this._vlcHost = vlcHost;
			this._vlcPort = vlcPort;
			this._vlcID = vlcID;
			this._vlcStartID = vlcStartID;
		}
			
		public string VLCHost{
			get{return _vlcHost;}
			set{_vlcHost=value;}
		}
		
		public string VLCPort{
			set{_vlcPort = value;}
			get{return _vlcPort;}
		}
		
		public int VLCStartID{
			set{_vlcStartID = value;}
			get{return _vlcStartID;}
		}
		
		/*public string VLCCommand{
			set{_vlcCommand = value;}
			get{return _vlcCommand;}
		}*/
		
		public int VLCId{
			set{_vlcID = Convert.ToInt16(value);}
			get{return _vlcID;}
		}
		
		public void PL_Play(){
			uriStr = this._vlcHost+":"+this._vlcPort+REMOTE_FILE+PLAY_COMMAND+"&id="+this._vlcID;
			/*using (StreamWriter sw = System.IO.File.AppendText(@"C:\start.txt")) 
	        {
				sw.WriteLine(uriStr);
	        }  */ 
			    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uriStr);
			    req.BeginGetResponse(new AsyncCallback(FinishWebRequest),req);
				/*try{
					HttpWebResponse response = (HttpWebResponse)req.GetResponse();
				}
				catch (WebException ex){
					using (StreamWriter sw = System.IO.File.AppendText(@"C:\start.txt")) 
			        {
						sw.WriteLine("PL_Play::"+ex);
			        } 
				}*/
			
			PL_PauseSeq();
		}
		public void PL_Stop(){
			//play first track (should be pause track) then pause it
			uriPlayStr = this._vlcHost+":"+this._vlcPort+REMOTE_FILE+PLAY_COMMAND+"&id="+this._vlcStartID;
			/*using (StreamWriter sw = System.IO.File.AppendText(@"C:\stop.txt")) 
	        {
				sw.WriteLine(uriPlayStr);
	        }  */ 
				HttpWebRequest reqPlay = 
					(HttpWebRequest)WebRequest.Create(uriPlayStr);
				reqPlay.BeginGetResponse(new AsyncCallback(FinishWebRequest),reqPlay);
				/*try{
					HttpWebResponse responsePlay = (HttpWebResponse)reqPlay.GetResponse();
				}
				catch (WebException ex){
					using (StreamWriter sw = System.IO.File.AppendText(@"C:\start.txt")) 
			        {
						sw.WriteLine("PL_Stop::"+ex);
			        } 	
				}*/
			this._vlcID = this._vlcStartID;
			PL_PauseSeq();
			
		}
		
		public void PL_PauseSeq(){
			/*using (StreamWriter sw = System.IO.File.AppendText(@"C:\stop.txt")) 
		        {
				sw.WriteLine("ID::"+this._vlcID.ToString());
		        }  */
			if(this._vlcID == this._vlcStartID){ //start id should be pause seq
				System.Threading.Thread.Sleep(500);
				//pause it
				uriPauseStr = this._vlcHost+":"+this._vlcPort+REMOTE_FILE+PAUSE_COMMAND+"&id="+this._vlcStartID;
				
		
	    			HttpWebRequest reqPause = 
						(HttpWebRequest)WebRequest.Create(uriPauseStr);
	    			reqPause.BeginGetResponse(new AsyncCallback(FinishWebRequest),reqPause);
					/*try{
						HttpWebResponse responsePause = (HttpWebResponse)reqPause.GetResponse();
					}
					catch (WebException ex){
						using (StreamWriter sw = System.IO.File.AppendText(@"C:\start.txt")) 
				        {
							sw.WriteLine("PL_PauseSeq::"+ex);
				        } 
					}*/

				
				
			}
		}
		public void FinishWebRequest(IAsyncResult result)
		{
			try{
		    	HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
		    }
			catch (WebException ex){
				using (StreamWriter sw = System.IO.File.AppendText(@"C:\start.txt")) 
		        {
					sw.WriteLine("FinishWebRequest::"+ex);
		        } 
			}
		}
	}
}
