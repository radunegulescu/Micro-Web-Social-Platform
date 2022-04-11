from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)

def press_add_friend_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[2]/form/button")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the add friend button")
        return False

def press_search_user_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/ol/form/button")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the search button")
        return False

def enter_user_name(driver, user_name):
    try:
        email_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, '//*[@id="search"]')))
        email_field.send_keys(user_name)
        return True
    except NoSuchElementException:
        logger.warning("Entering user name failed")
        return False