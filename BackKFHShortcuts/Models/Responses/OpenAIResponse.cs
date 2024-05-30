using BackKFHShortcuts.Models.Request;

namespace BackKFHShortcuts.Models.Responses
{
    public class OpenAIResponse
    {
        public string Id {  get; set; }
        public string Object {  get; set; }
        
        public int Created { get; set; }
        public string Model { get; set; }
        public string System_Fingerprint { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }
    }
    public class Choice
    {
        public int Index { get; set; }
        public MessageModel Message { get; set; }
        public string Finish_Reason {  get; set; }
    }
    public class Usage
    {
        public int Promot_Tokens { get; set; }
        public int Completion_Tokens { get; set; }
        public int Total_Tokens { get; set; }
    }
}
//{
//  "id": "chatcmpl-123",
//  "object": "chat.completion",
//  "created": 1677652288,
//  "model": "gpt-3.5-turbo-0125",
//  "system_fingerprint": "fp_44709d6fcb",
//  "choices": [
//    {
//        "index": 0,
//      "message": {
//            "role": "assistant",
//        "content": "\n\nHello there, how may I assist you today?"
//      },
//      "logprobs": null,
//      "finish_reason": "stop"
//    }
//  ],
//  "usage": {
//        "prompt_tokens": 9,
//    "completion_tokens": 12,
//    "total_tokens": 21
//  }
//}