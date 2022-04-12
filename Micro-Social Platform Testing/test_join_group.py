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


class Test_Join_Group():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def login(self):
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_join_group(self):
        self.login()

        driver.get("http://localhost:54805/Groups")
        time.sleep(2)
        assert(groups.join_group(driver, "Muzica"))
        time.sleep(5)
        assert(groups.show_group(driver, "Muzica"))
        assert(driver.current_url is not None and driver.current_url.startswith(
            "http://localhost:54805/Groups/Show"))

        time.sleep(10)
