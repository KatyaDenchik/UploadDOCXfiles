namespace ServiceLayer.SubModels
{
    public class EmailInformation
    {
        /// <summary>
        /// Gets or sets the body of the email
        /// </summary>
        public string Body { get; set; } 

        /// <summary>
        /// Gets or sets the target address of the email
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the sender name of the email
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the receiver name of the email
        /// </summary>
        public string To { get; set; }
    }
}
