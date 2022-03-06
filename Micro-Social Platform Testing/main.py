from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys

driver = webdriver.Chrome()
driver.get("http://localhost:54805/Account/Login")
email = driver.find_element(By.XPATH, "//*[@id='Email']")
email.send_keys("Hello!!")



