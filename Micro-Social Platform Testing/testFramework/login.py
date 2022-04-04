from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)


def enter_email(driver, email):
    try:
        email_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='Email']")))
        email_field.send_keys(email)
        return True
    except NoSuchElementException:
        logger.warning("Entering email failed")
        return False


def enter_password(driver, password):
    try:
        password_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='Password']")))
        password_field.send_keys(password)
        return True
    except NoSuchElementException:
        logger.warning("Entering password failed")
        return False


def check_remember_me(driver):
    try:
        remember_checkBox = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='RememberMe']")))
        remember_checkBox.click()
        return True
    except NoSuchElementException:
        logger.warning("Couldn't check the remember me box")
        return False


def press_login_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='loginForm']/form/div[4]/div/input")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the login button")
        return False
