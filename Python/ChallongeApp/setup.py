from setuptools import setup
setup(name='challongeToGoogle',
      version='0.1',
      description='Exports match results from challonge into a Google Doc Spreadsheet',
      author='Trevor Chapman',
      license='MIT',
      dependency_links=['https://github.com/russ-/pychallonge'],
      packages=['google-api-python-client', 'google-auth-httplib2', 'google-auth-oauthlib']
)
