from bs4 import BeautifulSoup
from urllib.request import urlopen
from selenium import webdriver
import os
import threading
import re
import csv


def main():
    driver = webdriver.PhantomJS(executable_path='C:/Windows/phantomjs.exe')
    driver.get("https://augsburgerjobs.de/job-detailsuche?l=Augsburg&a=2")
    bsObj = BeautifulSoup(driver.page_source,'html.parser')
    job_infos = bsObj.find_all('article')
    jobs = list()
    for job_info in job_infos:
        if 'job-posting' in job_info.get('class'):
            temp = job_info.strong.text.strip().split('\n')
            dt = (job_info.header.h3.a.get('title'), temp[0], job_info.header.time.text, temp[-1], job_info.header.h3.a.get('href'))
            jobs.append(dt)

    with open('jobs_from_Augsburgerjobs.csv', 'w') as csv_file:
        spamwriter = csv.writer(csv_file, delimiter=' ',quotechar=',', quoting=csv.QUOTE_MINIMAL)
        spamwriter.writerow(['job_name', 'job_host', 'publish_time', 'distance', 'link'])
        spamwriter.writerows(jobs)

"""
class DataStructure:
    def __init__(self, job_name, job_host, publish_time, distance, link):
        self.job_name = job_name
        self.job_host = job_host
        self.publish_time = publish_time
        self.distance = distance
        self.link = link
"""
if __name__ == '__main__':
    main()