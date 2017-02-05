using WpfControlLibrary.Table;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for EditRegDocTest and is intended
    ///to contain all EditRegDocTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EditRegDocTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for EditRegDoc Constructor
        ///</summary>
        [TestMethod()]
        public void EditRegDocConstructorTest()
        {
            ConnectTest.NewConnectionTest();
            bool _flagAdd = true; // TODO: Initialize to an appropriate value
            string _per_num = "14534"; // TODO: Initialize to an appropriate value
            Decimal _subdiv_id = 2; // TODO: Initialize to an appropriate value
            Decimal _transfer_id = 111779; // TODO: Initialize to an appropriate value
            Nullable<Decimal> _reg_doc_id = 2096426; // TODO: Initialize to an appropriate value
            Decimal _degree_id = 4; // TODO: Initialize to an appropriate value
            Decimal worker_id = 7; // TODO: Initialize to an appropriate value
            EditRegDoc target = new EditRegDoc(_flagAdd, _per_num, _subdiv_id, _transfer_id, _reg_doc_id, _degree_id, worker_id);
            target.ShowDialog();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /*/// <summary>
        ///A test for DateBegin_LostFocus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WpfControlLibrary.dll")]
        public void DateBegin_LostFocusTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            EditRegDoc_Accessor target = new EditRegDoc_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.DateBegin_LostFocus(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DateEnd_LostFocus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WpfControlLibrary.dll")]
        public void DateEnd_LostFocusTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            EditRegDoc_Accessor target = new EditRegDoc_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.DateEnd_LostFocus(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for InitializeComponent
        ///</summary>
        [TestMethod()]
        public void InitializeComponentTest()
        {
            bool _flagAdd = false; // TODO: Initialize to an appropriate value
            string _per_num = string.Empty; // TODO: Initialize to an appropriate value
            Decimal _subdiv_id = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal _transfer_id = new Decimal(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> _reg_doc_id = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Decimal _degree_id = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal worker_id = new Decimal(); // TODO: Initialize to an appropriate value
            EditRegDoc target = new EditRegDoc(_flagAdd, _per_num, _subdiv_id, _transfer_id, _reg_doc_id, _degree_id, worker_id); // TODO: Initialize to an appropriate value
            target.InitializeComponent();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Save_CanExecute
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WpfControlLibrary.dll")]
        public void Save_CanExecuteTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            EditRegDoc_Accessor target = new EditRegDoc_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CanExecuteRoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.Save_CanExecute(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Save_Executed
        ///</summary>
        /*[TestMethod()]
        [DeploymentItem("WpfControlLibrary.dll")]
        public void Save_ExecutedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            EditRegDoc_Accessor target = new EditRegDoc_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ExecutedRoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.Save_Executed(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }*/

        /// <summary>
        ///A test for System.Windows.Markup.IComponentConnector.Connect
        ///</summary>
        /*[TestMethod()]
        [DeploymentItem("WpfControlLibrary.dll")]
        public void ConnectTest()
        {
            bool _flagAdd = false; // TODO: Initialize to an appropriate value
            string _per_num = string.Empty; // TODO: Initialize to an appropriate value
            Decimal _subdiv_id = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal _transfer_id = new Decimal(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> _reg_doc_id = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Decimal _degree_id = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal worker_id = new Decimal(); // TODO: Initialize to an appropriate value
            IComponentConnector target = new EditRegDoc(_flagAdd, _per_num, _subdiv_id, _transfer_id, _reg_doc_id, _degree_id, worker_id); // TODO: Initialize to an appropriate value
            int connectionId = 0; // TODO: Initialize to an appropriate value
            object target1 = null; // TODO: Initialize to an appropriate value
            target.Connect(connectionId, target1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
        */
        /// <summary>
        ///A test for btExit_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WpfControlLibrary.dll")]
        public void btExit_ClickTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            EditRegDoc_Accessor target = new EditRegDoc_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.btExit_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
        
    }
}
