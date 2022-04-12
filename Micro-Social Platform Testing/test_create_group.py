import time
from testFramework import groups
from testFramework import login
from selenium import webdriver

'''
 User account for testing:
 andreineagu672@gmail.com
 Parola123!
'''
driver = webdriver.Chrome()


def setup_module():
    driver.get("http://localhost:54805/Account/Login")


class Test_Create_Group():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def login(self):
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_create_group(self):
        self.login()
        driver.get("http://localhost:54805/Groups")
        assert(groups.create_new_group(driver, "test01", "Test01Desc"))
        assert(groups.find_group(driver, "test01"))
