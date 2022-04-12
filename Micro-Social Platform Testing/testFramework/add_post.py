from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)

def press_add_post(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[1]/div/a")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the add post button")
        return False

def enter_title_post(driver,title):
    try:
        title_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='Title']")))
        title_field.send_keys(title)
        return True
    except NoSuchElementException:
        logger.warning("Entering title failed")
        return False

def enter_content_post(driver,content):
    try:
        content_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/form/div/div[3]/div[2]")))
        content_field.send_keys(content)
        return True
    except NoSuchElementException:
        logger.warning("Entering content failed")
        return False

def press_submit_post(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/form/button")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the submit post button")
        return False


#COMMENTS

def press_show_more(driver):
    try:
        showmore_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[2]/div[3]/a")))
        showmore_button.click()
        return True
    except:
        logger.warning("Couldn't press the press show more button")
        return False



def enter_comm_content(driver, content):
    try:
        content_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[3]/div/div[2]/div/form/div/textarea")))
        content_field.send_keys(content)
        return True
    except NoSuchElementException:
        logger.warning("Entering content failed")
        return False


def press_add_comment(driver):
    try:
        addcomm_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div[3]/div/div[2]/div/form/div/button")))
        addcomm_button.click()
        return True
    except:
        logger.warning("Couldn't press the add comm button")
        return False


