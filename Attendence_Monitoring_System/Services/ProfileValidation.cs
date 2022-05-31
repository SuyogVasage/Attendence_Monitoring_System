using System.Text.RegularExpressions;

namespace Attendence_Monitoring_System.Services
{
    public class ProfileValidation
    {
        public bool Validation(UserDetail userDetail, out string ErrorMessage)
        {
            switch(userDetail.KeyName)
            {
                case "Email Id":
                    Regex re = new Regex("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9+.-]+$");
                    if (!re.IsMatch(Convert.ToString(userDetail.Value)))
                    {
                        ErrorMessage = "Please Enter Correct Email ID";
                        return false;
                    }
                    break;
                case "Name":
                    Regex res = new Regex("^([a-zA-Z]+( [a-zA-Z]+)+)$");
                    if (!res.IsMatch(Convert.ToString(userDetail.Value)))
                    {
                        ErrorMessage = "Please Enter Full Name";
                        return false;
                    }
                    break;
                case "MobileNo":
                    Regex res1 = new Regex(@"^[0-9]{10}$");
                    if (!res1.IsMatch(Convert.ToString(userDetail.Value)))
                    {
                        ErrorMessage = "Mobile Number Should be of 10 digits";
                        return false;
                    }
                    break;
                case "Date of Birth":
                    Regex res2 = new Regex(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$");
                    if (!res2.IsMatch(Convert.ToString(userDetail.Value)))
                    {
                        ErrorMessage = "DOB should be in DD/MM/YYYY format";
                        return false;
                    }
                    break;
                case "Gender":
                    if(userDetail.Value != "Male" && userDetail.Value != "Female")
                    {
                        ErrorMessage = "Gender can be only Male or Female";
                        return false;
                    }
                    break;
                case "Blood Group":
                    Regex res3 = new Regex(@"^(A | B | AB | O)[+-]$");
                    if (!res3.IsMatch(Convert.ToString(userDetail.Value)))
                    {
                        ErrorMessage = "Please Enter Correct Blood Group";
                        return false;
                    }
                    break;
                case "Maritial Status":
                    if (userDetail.Value != "Single" && userDetail.Value != "Married")
                    {
                        ErrorMessage = "Maritial Status can be only Single or Married";
                        return false;
                    }
                    break;
                case "Fresher":
                    if (userDetail.Value != "Yes" && userDetail.Value != "No")
                    {
                        ErrorMessage = "Please Enter Yes or No";
                        return false;
                    }
                    break;
                case null:
                    ErrorMessage = "";
                    return true;

            }
            if(userDetail.KeyName == "SSC Year of Completion" || userDetail.KeyName == "HSC Year of Completion" || userDetail.KeyName == "Degree Year of Completion")
            {
                Regex res4 = new Regex(@"^(19 | 20)\d{ 2}$");
                if (!res4.IsMatch(Convert.ToString(userDetail.Value)))
                {
                    ErrorMessage = "Please Enter Correct Year";
                    return false;
                }
            }
            if (userDetail.KeyName == "Degree Percentage" || userDetail.KeyName == "HSC Percentage" || userDetail.KeyName == "SSC Percentage")
            {
                Regex res4 = new Regex(@"^[1-9][0-9]?$|^100$");
                if (!res4.IsMatch(Convert.ToString(userDetail.Value)))
                {
                    ErrorMessage = "Please Enter Correct Percentage";
                    return false;
                }
            }
                         ErrorMessage = "";
                return true;
        }

       
    }
}
