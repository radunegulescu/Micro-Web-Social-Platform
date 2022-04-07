from testFramework import register
from selenium import webdriver

driver = webdriver.Chrome()


def setup_module():
    driver.get("http://localhost:54805/Account/Register")


class Test_Register():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Register")

    def test_register_successful(self):
        assert(register.enter_email(driver, "testRegister3@gmail.com"))
        assert(register.enter_username(driver, "testRegister"))
        assert(register.enter_password(driver, "Parola123!"))
        assert(register.enter_confirm_password(driver, "Parola123!"))
        assert(register.press_register_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_register_failed_email(self):
        assert (register.enter_email(driver, "testRegister"))
        assert (register.enter_username(driver, "testRegister"))
        assert (register.enter_password(driver, "Parola123!"))
        assert (register.enter_confirm_password(driver, "Parola123!"))
        assert (register.press_register_button(driver))
        assert (driver.current_url is not None and driver.current_url ==
                "http://localhost:54805/Account/Register")

    def test_register_failed_username(self):
        assert (register.enter_email(driver, "testRegister"))
        assert (register.enter_username(driver, ""))
        assert (register.enter_password(driver, "Parola123!"))
        assert (register.enter_confirm_password(driver, "Parola123!"))
        assert (register.press_register_button(driver))
        assert (driver.current_url is not None and driver.current_url ==
                "http://localhost:54805/Account/Register")

    def test_register_failed_password(self):
        assert (register.enter_email(driver, "testRegister"))
        assert (register.enter_username(driver, ""))
        assert (register.enter_password(driver, "Parola123!1"))
        assert (register.enter_confirm_password(driver, "Parola123!"))
        assert (register.press_register_button(driver))
        assert (driver.current_url is not None and driver.current_url ==
                "http://localhost:54805/Account/Register")
