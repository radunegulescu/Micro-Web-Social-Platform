from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
import pytest


class TestLogin():
    # driver = webdriver.Chrome()
    # driver.get("http://localhost:54805/Account/Login")
    # email = driver.find_element(By.XPATH, "//*[@id='Email']")
    # email.send_keys("radu@test.com")
    # password = driver.find_element(By.XPATH, "//*[@id='Password']")
    # password.send_keys("Parola123.")
    # login = driver.find_element(By.XPATH, "//*[@id='loginForm']/form/div[4]/div/input")
    # login.click()

    def click(self, driver):
        try:
            login = driver.find_element(By.XPATH, "//*[@id='loginForm']/form/div[4]/div/input")
            login.click()
            return True
        except Exception:
            return False

    def testLoginSuccessful(self):
        driver = webdriver.Chrome()
        driver.get("http://localhost:54805/Account/Login")
        email = driver.find_element(By.XPATH, "//*[@id='Email']")
        email.send_keys("radu@test.com")
        password = driver.find_element(By.XPATH, "//*[@id='Password']")
        password.send_keys("Parola121.")
        assert(self.click(driver))


@pytest.fixture(scope="function", autouse=True)
def tearDown():
    pass