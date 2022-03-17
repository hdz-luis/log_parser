using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutSystems_Log_Parser
{
    public partial class Form3 : Form
    {
        string httpCode = "";
        string meaning = "";
        string description = "";

        public Form3()
        {
            InitializeComponent();
        }

        // Instantiate a new 2D string array.
        //the HTTP code, its meaning, and its description
        string[,] array = new string[63, 3]
        {
            {"100", "Continue", "The 100 Continue status code means that the initial part of the request has been received by the server and that the client should proceed with the request or ignore the response if the request has already finished."},
            {"101", "Switching protocols", "The 101 Switching protocols status code means that the server understands the Upgrade header field request and indicates which protocol it is switching to."},
            {"102", "Processing", "The 102 Processing status code means that the server has accepted the full request but has not yet completed it and no response is available as of yet."},
            {"103", "Early Hints", "The 103 Early hints status code is intended to be used to allow the user agent to preload resources, while the server prepares a response. It is intended to be primarily used with the Link Header."},
            {"200", "OK", "The 200 OK status code means that the request was successful, but the meaning of success depends on the request method used:" + Environment.NewLine + Environment.NewLine + "GET: The requested resource has been fetched and transmitted to the message body." + Environment.NewLine + "HEAD: The header fields from the requested resource are sent in without the message body." + Environment.NewLine + "POST or PUT: A description of the result of the action is transmitted to the message body." + Environment.NewLine + "TRACE: The request messages, as received by the server, will be included in the message body"},
            {"201", "Created", "The 201 Created status code means that the request was successfully fulfilled and resulted in one or possibly multiple new resources being created."},
            {"202", "Accepted", "The 202 Accepted status code means that the request has been accepted for processing, but the processing has not been finished yet. The request may or may not be completed when the processing eventually takes place."},
            {"203", "Non-Authoritative Information", "The 203 Non-Authoritative Information status code means that the request was successful. However, the meta-information that has been received is different from the one on the origin server and has instead been collected from a 3rd party or local copy. When not used for backups or mirrors of another resource a 200 OK response is preferable."},
            {"204", "No Content", "The 204 No Content status code means that while the server has successfully fulfilled the request, there is no available content for this request. But the user agent might want to update its currently cached headers for this resource, for the new one. "},
            {"205", "Reset Content", "The 205 Reset Content status code means that the user should reset the document that sent this request."},
            {"206", "Partial Content", "The 206 Partial Content response code is a response to a Range header sent from the client when requesting only a part of the resource."},
            {"207", "Multi-Status", "The 207 Multi-Status status code conveys information about multiple resources, in situation when multiple status codes are appropriate."},
            {"208", "Already Reported", "The 208 Already Reported status code is used inside the response element DAV: propstat, in order to avoid enumerating the internal members of multiple bindings to the same collection repeatedly."},
            {"226", "IM Used", "The 226 IM response code means that the server has successfully fulfilled a GET request for the resource, and the response is a representation of the result of one or multiple instance-manipulations applied to the current instance."},
            {"300", "Multiple Choices", "The 300 Multiple Choices status code means that the request has multiple possible responses and the user/user agent should choose one."},
            {"301", "Moved Permanently", "The 301 Moved Permanently response code means that the target resource has been assigned a new permanent URL and any references to this resources in the future should use one of the URLs included in the response."},
            {"302", "Found (Previously \"Moved Temporarily\")", "The 302 Found status code, previously known as “Moved temporarily”, means that the URI of the request has been changed temporarily, and since changes can be made to the URI in the future, the effective request URI should be used for future requests."},
            {"303", "See Other", "The 303 See Other response code is sent by the server in order to direct the client to get the requested resource at another URI with a GET request."},
            {"304", "Not Modified", "The 304 Not Modified response code informs the client that the response has not been modified. This means that the client can continue to use the already present, cached version of the response."},
            {"305", "Use Proxy", "The 305 Use Proxy status code instructs a client that it should connect to a proxy and then repeat the same request there. This response code is deprecated due to security concerns."},
            {"306", "Switch Proxy", "The 306 Switch proxy status code is no longer in use. It was used to inform the client that the subsequent requests should use the specified proxy."},
            {"307", "Temporary Redirect", "The 307 Temporary Redirect status code gets sent by the server in order to direct the client to the requested resource at another URI. The request method, however, must not be changed."},
            {"308", "Permanent Redirect", "The 308 Permanent Redirect status code means that the requested resource has been permanently assigned a new URI and future references to the resource should be made by using one of the enclosed URIs."},
            {"400", "Bad Request", "The 400 Bad Request status code means that the server could not understand the request because of invalid syntax."},
            {"401", "Unauthorized", "The 401 Unauthorized status code means that the request has not been applied because the server requires user authentication."},
            {"402", "Payment Required", "The 402 Payment Required status code is a response reserved for future use. It was originally created to be implemented in digital payment systems, however, it is rarely used and a standard convention of using it does not exist."},
            {"403", "Forbidden", "The 403 Forbidden status code means that the client request has been rejected because the client does not have rights to access the content. Unlike a 401 error, the client's identity is known to the server, but since they are not authorized to view the content, giving the proper response is rejected by the server."},
            {"404", "Not Found", "The 404 Not Found status code means that the server either did not find a current representation for the requested resource or is trying to hide its existence from an unauthorized client."},
            {"405", "Method Not Allowed", "The 405 Method Not Allowed status code means that while the server knows the request method, the method has been disabled and can not be used."},
            {"406", "Not Acceptable", "The 406 Not Acceptable status code is sent by the server when it does not find any content following the criteria given by the user agent."},
            {"407", "Proxy Authentication Required", "The 407 Proxy Authentication Required status code means that the client must first be authenticated by a proxy (similar to a 401)."},
            {"408", "Request Timeout", "The 408 Request Timeout status code means that the server did not receive a complete request in the time that it prepared to wait."},
            {"409", "Conflict", "The 409 Conflict status code means that the request could not be fulfilled due to a conflict with the current state of the target resource and is used in situations where the user might be able to resubmit the request after resolving the conflict."},
            {"410", "Gone", "The 410 Gone status code means that the target resource has been deleted and the condition seems to be permanent. "},
            {"411", "Length Required", "The 411 Length Required status code means that the server has rejected the request because it requires the Content-Length header field to be defined."},
            {"412", "Precondition Failed", "The 412 Precondition Failed status code means the server does not meet one or multiple preconditions that were indicated in the request header fields."},
            {"413", "Payload Too Large", "The 413 Payload Too Large status code means the server refuses to process the request because the request payload is larger than the server is able or willing to process. While the server may close the connection to prevent the client from continuing the request, it should generate a Retry-After header field and after how long can the client retry."},
            {"414", "URI Too Long", "The 414 URI Too Long status code means that the server is refusing to service the request because the request-target was longer than the server was willing to interpret."},
            {"415", "Unsupported Media Type", "The 415 Unsupported Media Type status code means that the server is rejecting the request because it does not support the media format of the requested data."},
            {"416", "Range Not Satisfiable", "The 416 Range Not Satisfiable status code means that the range specified in the Range header field of the request can't be fulfilled. The reason might be that the given range is outside the size of the target URI's data."},
            {"417", "Expectation Failed", "The 417 Expectation Failed status code means that the Expectation indicated by the Expect request-header field could not be met by the server."},
            {"418", "I'm a Teapot", "The 418 I'm a Teapot status code means that the server refuses to brew coffee because it is, in fact, a teapot. (It is a reference to a 1998 April Fools' joke called ''Hyper Text Coffee Pot Control Protocol'')."},
            {"421", "Misdirected Request", "The 421 Misdirected Request status code means that the client request was directed at a server that is not configured to produce a response."},
            {"422", "Unprocessable Entity", "The 422 Unprocessable Entity status code means that while the request was well-formed, the server was unable to follow it, due to semantic errors."},
            {"423", "Locked", "The 423 Locked status code means that the resource that is being accessed is locked."},
            {"424", "Failed Dependency", "The 424 Failed Dependency status code means that the request failed due to the failure of a previous request."},
            {"425", "Too Early", "The 425 Too Early status code means that the server is not willing to risk processing a request that might be replayed."},
            {"426", "Upgrade Required", "The 426 Upgrade Required status code means that while the server refuses to perform the given request using the current protocol, it might be willing to do so after the client has been upgraded to a different protocol."},
            {"428", "Precondition Required", "The 428 Precondition Required status code means that the origin server requires the request to be conditional."},
            {"429", "Too Many Requests", "The 429 Too Many Requests response code means that in the given time, the user has sent too many requests."},
            {"431", "Request Header Fields Too Large", "The 431 Request Header Fields Too Large means that the server is not willing to process the request because its header fields are indeed too large, however, the request may be submitted again once the size of the request header fields is reduced."},
            {"451", "Unavailable For Legal Reasons", "The 451 Unavailable For Legal Reasons response code means that the user has requested an illegal resource (such as pages and sites blocked by the government)."},
            {"500", "Internal Server Error", "The 500 Internal Server Error status code means that the server has encountered a situation that it does not know how to handle."},
            {"501", "Not Implemented", "The 501 Not Implemented response code means that the request can not be handled because it is not supported by the server."},
            {"502", "Bad Gateway", "The 502 Bad Gateway response code means that the server received an invalid response while working as a gateway to handle the response."},
            {"503", "Service Unavailable", "The 503 Service Unavailable response code means that the server is currently not ready to handle the request. This is a common occurrence when the server is down for maintenance or is overloaded."},
            {"504", "Gateway Timeout", "The 504 Gateway Timeout response code means that the server acting as a gateway could not get a response time."},
            {"505", "HTTP Version Not Supported", "The 505 HTTP Version Not Supported response code means that the version of HTTP used in the request is not supported by the server."},
            {"506", "Variant Also Negotiates", "The 506 Variant Also Negotiates response code means that the server has the following internal configuration error: The chosen variant resource is configured to engage in transparent negotiation itself, therefore it cannot be a proper endpoint in the negotiation process."},
            {"507", "Insufficient Storage", "The 507 Insufficient Storage status code means that the method could not be performed on the resource because the server is not able to store the representation that would be needed to complete the request successfully."},
            {"508", "Loop Detected", "The 508 Loop Detected response code means that the server has detected an infinite loop while processing the request."},
            {"510", "Not Extended", "The 510 Not Extended response code means that further extensions are required for the server to be able to fulfil the request."},
            {"511", "Network Authentication Required", "The 511 Network Authentication Required response code indicates that the client needs to authenticate to gain network access. "}
        };

        private void listViewCodes_Click(object sender, EventArgs e)
        {
            //when clicking on the table, pass the values from the selected row to the textboxes
            httpCode = listViewCodes.SelectedItems[0].SubItems[0].Text;
            meaning = listViewCodes.SelectedItems[0].SubItems[1].Text;
            description = listViewCodes.SelectedItems[0].SubItems[2].Text;

            txtBoxHTTPcode.Text = httpCode;
            txtBoxMeaning.Text = meaning;
            txtBoxDescription.Text = description;
        }

        private void txtBoxSearchHTTPcode_TextChanged(object sender, EventArgs e)
        {
            //make sure there are at least two digits before starting to search for similar HTTP codes
            if (txtBoxSearchHTTPcode.Text.Length <= 1)
            {
                listViewCodes.Clear();
                txtBoxHTTPcode.Clear();
                txtBoxMeaning.Clear();
                txtBoxDescription.Clear();
            }
            else if (txtBoxSearchHTTPcode.Text.Length > 1)
            {
                listViewCodes.Clear();
                listViewCodes.View = View.Details;
                listViewCodes.Columns.Add("HTTP Code");
                listViewCodes.Columns.Add("Meaning");
                listViewCodes.Columns.Add("Description");
                listViewCodes.GridLines = true;

                for (int i = 0; i < array.GetLength(0); i++)
                {
                    if (array[i, 0].Contains(txtBoxSearchHTTPcode.Text))
                    {
                        listViewCodes.Items.Add(new ListViewItem(new string[] { array[i, 0], array[i, 1], array[i, 2] }));
                    }
                }
            }
        }
    }
}
