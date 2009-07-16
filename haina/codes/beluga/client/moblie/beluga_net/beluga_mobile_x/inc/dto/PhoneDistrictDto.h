#ifndef __PHONEDISTRICTDTO_H__
#define __PHONEDISTRICTDTO_H__
#include <string>

using namespace std;

class PhoneDistrictDto
{
public:
	void	setDistrictNumber(string aNumber);
	string	getDistrictNumber();
	
	void	setRange(string aRange);
	string	getRange();

	void	setFeeType(string aFeeType);
	string	getFeeType();

	void	setDistrictCity(string aCity);
	string	getDistrictCity();

	void	setDistrictProvince(string aProvince);
	string	getDistrictProvince();

	void	setUpdateFlg(int aFlg);
	int		getUpdateFlg();

	void	setWeatherCityCode(string aWeatherCityCode);
	string	getWeatherCityCode();

private:
	string		districtNumber;			/*����*/				
	string		range;					/*�ֻ�ǰ7λ��Χ*/				
	string		feeType;				/*�ʷ�����*/				
	string		districtCity;			/*����*/				
	string		districtProvince;		/*ʡ��*/				
	int			updateFlg;				/*���±�־*/				
	string		weatherCityCode;		/*��������*/				
	
};

#endif /* __PHONEDISTRICTDTO_H__ */
