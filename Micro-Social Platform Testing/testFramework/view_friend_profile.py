from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)

def press_only_friends_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/ol/a[1]")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the only friends button")
        return False

def press_view_pofile_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[3]/a")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the view profile button")
        return False

def test_profile_name(driver, name):
    try:
        profile_name = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[2]/span")))
        if profile_name.text == name:
            return True
        return False
    except:
        logger.warning("Couldn't view the profile")
        return False