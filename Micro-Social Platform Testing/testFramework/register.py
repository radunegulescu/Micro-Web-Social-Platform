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


def enter_username(driver, username):
    try:
        username_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='NameUser']")))
        username_field.send_keys(username)
        return True
    except NoSuchElementException:
        logger.warning("Entering username failed")
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


def enter_confirm_password(driver, password):
    try:
        password_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='ConfirmPassword']")))
        password_field.send_keys(password)
        return True
    except NoSuchElementException:
        logger.warning("Entering password failed")
        return False


def press_register_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/form/div[6]/div/input")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the register button")
        return False
