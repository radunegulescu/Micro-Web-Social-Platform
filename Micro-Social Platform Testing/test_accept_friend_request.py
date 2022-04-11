from testFramework import login
from testFramework import accept_friend_request
from selenium import webdriver

driver = webdriver.Chrome()

def setup_module():
    driver.get("http://localhost:54805/Account/Login")

class Test_Accept_Friend_Request():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def test_login_successful(self):
        assert(login.enter_email(driver, "marinela@gmail.com"))
        assert(login.enter_password(driver, "*123Marinela"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_accept_friend_request_successful(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_accept_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))

    def test_reject_friend_request_successful(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_reject_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))

    def test_reject_friend_request_failed(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_reject_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))

    def test_accept_friend_request_failed(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_accept_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))
