using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TravelExtravaganza
{
    public partial class frmCruiseBuilder : Form
    {
        public frmCruiseBuilder()
        {
            InitializeComponent();
        }

        private void frmCruiseBuilder_Load(object sender, EventArgs e)
        {
            //put controls into lists to modify their properties later
            List<CheckBox> services = new List<CheckBox>()
        {chkService1, chkService2, chkService3, chkService4, chkService5, chkService6};

            List<PictureBox> pictures = new List<PictureBox>()
        {picService1, picService2, picService3, picService4, picService5, picService6};

            //put tag from splash form into string variable for use with SetCruiseControls();
            string cruiseLocation = (string)Application.OpenForms["frmSplash"].Tag;

            SetCruiseControls(cruiseLocation, services, pictures);
        }

        /* Below are arrays for control properties and prices.
         * Each row of a particular rectangular array corresponds to a cruise type.
         * So row 0 = caribbean row 1 = norway and row 2 = lake minnetonka
         * so my switch statement now only needs to assign indexRow */

         /* in order for SetCruiseControls to work properly you need my updated code
         * for splash form which I'll share as well */

        int indexRow; //variable used in SetCruiseControls

        //price variables and array
        decimal cruiseBasePrice;
        decimal[,] prices = new decimal[3, 7] {
            {828.96m, 80m, 150m, 112m, 182m, 200m, 114m},
            {2380.7m, 200m, 100m, 119m, 250m, 300m, 200m},
            {36.95m, 15m, 25m, 30m, 10.50m, 5, 14.99m} };

        //arrays that hold control properties
        string[,] serviceText = new string[3, 7] {
            {"Caribbean", "Youth and Teen Program \t$80.00", "Tickets to Onboard Entertainment \t$150.00",
                "Party Pass \t$112.00", "Snorkeling \t$182.00", "Casino Pass \t200.00", "Whale Watching \t114.00"},
            {"Norway", "Bergen Walking Tour \t$200.00", "Goat Cheese Making Class \t100.00", 
                "Wine Tasting \t$119.00", "Kayaking Geirangerfjord \t$250.00", 
                "Guided Art History Munch Museum Tour \t$300.00", "Fjord Hiking \t$200.00"},
            {"Lake Minnetonka", "Visiting Wayzata \t$15.00", "Fishing \t$25.00", "Dinner on Steamboat Minnehaha \t$30.00",
                "Pie Eating Contest \t$10.50", "Big Island \t$5.00", "Duck and Loon Watching Tour \t$14.99"} };

        string[] images = new string[6]
            {"Service1.png", "Service2.png", "Service3.png", "Service4.png", "Service5.png", "Service6.png"};

        
        //can use index of prices array to get the correct index for which image to show
        private void SetCruiseControls(string cruise, List<CheckBox> services, List<PictureBox> pictures)
        {
            switch (cruise)
            {
                case "Caribbean":
                    indexRow = 0;
                    break;
                case "Norway":
                    indexRow = 1;;
                    break;
                case "LakeMinnetonka":
                    indexRow = 2;
                    break;
                default:
                    indexRow = 0;
                    break;
            }

            //counters for control property assignment
            int i = 1;
            int j = 0;

            this.Text = serviceText[indexRow, 0];

            //assigns correct Text and index value for each service
            //so service.Tag now holds the index value for the correct price
            //and can subtract 1 from that index value to get
            //the correct index value for the corresponding picture box
            foreach (CheckBox service in services)
            {
                service.Text = serviceText[indexRow, i];
                service.Tag = i;
                i++;
            }

            //assigns correct image for each picture box
            //I also put the index value in picture.Tag in case that's helpful?
            //because you might not be able to access my picturebox object list
            //since it has to be in the load event of the form
            foreach (PictureBox picture in pictures)
            {
                picture.Image = Image.FromFile(Application.StartupPath + "//images//" +
            cruise + images[j]);
                picture.Tag = j;
                j++;
            }

            //puts correct cruise base price into cruiseBasePrice variable
            cruiseBasePrice = prices[indexRow, 0];

            lstQuotes.Items.Clear();
            lstQuotes.Items.Add(cruise + " Cruise Base Package \t" + cruiseBasePrice.ToString("c"));

            lblTotalPrice.Text = cruiseBasePrice.ToString("c");
        }


        private void chkService_CheckedChanged(object sender, EventArgs e)
        {
            //check box functionality
            var SenderObject = (CheckBox)sender;
            string activity = SenderObject.Text;

            //ED NOTE: I edited this part - so now you have both an index and an activity price to work with
            decimal activityPrice = prices[indexRow, (int)SenderObject.Tag];
            int index = ((int)SenderObject.Tag) - 1;

            if (SenderObject.Checked)
            {
                lstQuotes.Items.Add(activity);
                cruiseBasePrice += activityPrice;
                lblTotalPrice.Text = cruiseBasePrice.ToString();
                //int index = prices.IndexOf(25);
                //PictureBox.Show();
                
                //ED NOTE: so your index variable now holds an index
                //that corresponds to the number I've assigned to each picture box's tag
                //could maybe do an if statement that shows the picture box if 
                //its tag == index?
                
            }
            if (!SenderObject.Checked)
            {
                lstQuotes.Items.Remove(activity);
                cruiseBasePrice -= activityPrice;
                lblTotalPrice.Text = cruiseBasePrice.ToString();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //TO DO: check box functionality
        //includes populating list box with services & their prices

        //TO DO: calculation method to get the total

        //TO DO: Data validation? Exception handling.
    }
}
