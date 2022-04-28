import time
from testFramework import groups
from testFramework import login
from testFramework import logoff
from selenium import webdriver

'''
 User account for testing:
 andreineagu672@gmail.com
 Parola123!
'''
driver = webdriver.Chrome()


def setup_module():
    driver.get("http://localhost:54805/Account/Login")
    driver.maximize_window()


class Test_Integration_Group():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def login(self, email, parola):
        assert(login.enter_email(driver, email))
        assert(login.enter_password(driver, parola))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_join_group(self):
        self.login("andreineagu672@gmail.com", "Parola123!")

        driver.get("http://localhost:54805/Groups")

        assert(groups.create_new_group(driver, "test03", "test03"))
        time.sleep(2)
        assert(groups.join_group(driver, "test03"))
        time.sleep(2)
        assert(groups.show_group(driver, "test03"))
        time.sleep(2)
        assert(groups.edit_group(driver, "", "test"))
        time.sleep(2)
        assert(logoff.press_logoff_button(driver))

        self.login("user@gmail.com", "!Username06")

        driver.get("http://localhost:54805/Groups")
        time.sleep(2)
        assert(groups.join_group(driver, "test03"))
        time.sleep(5)
        assert(groups.show_group(driver, "test03"))

        assert(logoff.press_logoff_button(driver))

        self.login("andreineagu672@gmail.com", "Parola123!")

        driver.get("http://localhost:54805/Groups")
        assert(groups.show_group(driver, "test03"))
        assert(groups.delete_group(driver))
