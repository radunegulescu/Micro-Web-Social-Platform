from testFramework import login
from selenium import webdriver
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By

driver = webdriver.Chrome()


def setup_module():
    driver.get("http://localhost:54805/Account/Login")


class Test_Login():
    def teardown_method(self):
        # if not driver.current_url.startswith(
        #         'http://localhost:54805/Account/Login'):
        #     logoff_button = WebDriverWait(driver, 10).until(
        #         EC.visibility_of_element_located((By.XPATH, "//*[@id='logoutForm']/ul/li[2]/a")))
        #     logoff_button.click()
        driver.get("http://localhost:54805/Account/Login")

    def test_login_succesfull(self):
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_login_failed_email(self):
        assert(login.enter_email(driver, "andreineagu672"))
        assert(login.enter_password(driver, "Parola1234!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))

    def test_login_failed_email_empty(self):
        assert(login.enter_email(driver, ""))
        assert(login.enter_password(driver, "Parola1234!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))

    def test_login_failed_password(self):
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola1234!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Account/Login'))
